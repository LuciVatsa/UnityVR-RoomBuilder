#include "VIVEPORT.h"
#include "viveport_api.h"
#include "viveport_ext_api.h"
#include <string>
#include <iostream>

void InitHandler(int nResult);
void ShutdownHandler(int nResult);

void GetLicenseHandler(
    const char* message,
    const char* signature
    );
std::string DecodeBase64(const std::string& encoded);

void IsTokenReadyHandler(int nResult);
void GetSessionTokenHandler(
    int nResult,
    const char* message
    );
void IsDlcReadyHandler(int nResult);
void IsSubscriptionReadyHandler(int nResult, const char* message);

static ViveportApi::LicenseChecker* license_checker;
//static TArray<ViveportApi::LicenseChecker*> s_license_checkers;
static ViveportApiStatus* init_callback;
static ViveportApiStatus* shutdown_callback;
static ViveportApiStatus* token_is_ready_callback;
static ViveportTokenStatus* session_token_callback;
static ViveportApiStatus* dlc_is_ready_callback;
static ViveportSubscriptionStatus* subscription_is_ready_callback;
static std::string s_app_id;
static std::string s_app_key;
static std::string s_message;
static std::string s_signature;
ViveportTokenDemo::MySessionTokenCallback g_session_token_callback;

int ViveportApi::Init(
    ViveportApiStatus *callback,
    const std::string app_id
    )
{
    init_callback = callback;
    return ViveportAPI()->Init(
        InitHandler,
        s_app_id.c_str());
}

void ViveportApi::GetLicense(
    LicenseChecker *my_license_checker,
    const std::string app_id,
    const std::string app_key
    )
{
    license_checker = my_license_checker;
    s_app_id = app_id;
    s_app_key = app_key;
    ViveportAPI()->GetLicense(
        GetLicenseHandler,
        s_app_id.c_str(),
        s_app_key.c_str()
        );
}

int ViveportApi::Shutdown(ViveportApiStatus *callback)
{
    shutdown_callback = callback;
    return ViveportAPI()->Shutdown(ShutdownHandler);
}

std::string ViveportApi::Version()
{
    auto native_version = std::string("Native API version: ")
        .append(ViveportAPI()->Version())
        .append(", Native Ext API version: ")
        .append(ViveportExtApi()->Version());
    return native_version.c_str();
}

void InitHandler(int nResult)
{
    return nResult == 0 ? init_callback->OnSuccess() : init_callback->OnFailure(nResult);
}

void ShutdownHandler(int nResult)
{
    return nResult == 0 ? shutdown_callback->OnSuccess() : shutdown_callback->OnFailure(nResult);
}

void GetLicenseHandler(
    const char* message,
    const char* signature
    )
{
    auto is_verified = message != nullptr && strlen(message) > 0;
    if (!is_verified)
    {
        if (license_checker != NULL) {
            //auto checker = license_checker;
            license_checker->OnFailure(
                90003,
                std::string("License message is empty")
                );
        }
        return;
    }

    s_message = std::string(message);
    s_signature = std::string(signature);

    std::cout << "app_id:" << s_app_id << std::endl;
    std::cout << "app_key:" << s_app_key << std::endl;
    std::cout << "message:" << s_message << std::endl;
    std::cout << "signature:" << s_signature << std::endl;

    auto result1 = ViveportExtLicense()->VerifyMessage(
        s_app_id.c_str(),
        s_app_key.c_str(),
        s_message.c_str(),
        s_signature.c_str()
        );
    auto wrong_app_id(s_app_id);
    wrong_app_id.append("_");
    auto result2 = ViveportExtLicense()->VerifyMessage(
        wrong_app_id.c_str(),
        s_app_key.c_str(),
        s_message.c_str(),
        s_signature.c_str()
        );

    is_verified = result1 > 0 && result2 == 0;
    if (!is_verified)
    {
        if (license_checker != NULL) {
            //auto checker = license_checker;
            license_checker->OnFailure(
                90001,
                std::string("License verification failed")
                );
        }
        return;
    }

    std::string msg(s_message.c_str());
    int index = msg.find('\n');
    if (index == std::string::npos) {
        // cannot find json start!!
        std::cout << "cannot find json start!!" << std::endl;
        index = -1;
    }
    auto encoded_json_message = msg.substr(index + 1);
    std::cout << "encoded_json_message:" << encoded_json_message << std::endl;

    auto json_message = DecodeBase64(std::string(encoded_json_message));
    std::cout << "json_message:" << json_message << std::endl;
}

std::string DecodeBase64(const std::string& encoded)
{
    static const char reverse_table[128] = {
        64, 64, 64, 64, 64, 64, 64, 64, 64, 64, 64, 64, 64, 64, 64, 64,
        64, 64, 64, 64, 64, 64, 64, 64, 64, 64, 64, 64, 64, 64, 64, 64,
        64, 64, 64, 64, 64, 64, 64, 64, 64, 64, 64, 62, 64, 64, 64, 63,
        52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 64, 64, 64, 64, 64, 64,
        64,  0,  1,  2,  3,  4,  5,  6,  7,  8,  9, 10, 11, 12, 13, 14,
        15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 64, 64, 64, 64, 64,
        64, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40,
        41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 64, 64, 64, 64, 64
    };

    std::string result;
    const auto last = encoded.end();
    auto bits_collected = 0;
    unsigned int accumulator = 0;

    for (auto i = encoded.begin(); i != last; ++i) {
        const int c = *i;
        if (isspace(c) || c == '=') {
            // Skip whitespace and padding. Be liberal in what you accept.
            continue;
        }
        if ((c > 127) || (c < 0) || (reverse_table[c] > 63)) {
            throw std::invalid_argument("This contains characters not legal in a base64 encoded string.");
        }
        accumulator = (accumulator << 6) | reverse_table[c];
        bits_collected += 6;
        if (bits_collected >= 8) {
            bits_collected -= 8;
            result += static_cast<char>((accumulator >> bits_collected) & 0xffu);
        }
    }
    return result;
}

ViveportApiDemo::ViveportApiDemo()
{
    std::cout << "ver:" << ViveportApi::Version() << std::endl;
    ViveportApi::Init(&myInitCallback, APP_ID);
}

void ViveportApiDemo::Start()
{
    ViveportApi::GetLicense(&myLicenseChecker, APP_ID, APP_KEY);
}

ViveportApiDemo::~ViveportApiDemo()
{
    ViveportApi::Shutdown(&myShutdownCallback);
}

void ViveportApiDemo::MyLicenseChecker::OnSuccess(
    long long issue_time,
    long long expiration_time,
    int latest_version,
    bool update_required
    )
{
    char result[256] = { '\0' };
    sprintf_s(
        result,
        "Verify OK!\n issue_time=%lld,\n expiration_time=%lld,\n latest_version=%d,\n update_required=%d",
        issue_time,
        expiration_time,
        latest_version,
        update_required
        );

    std::cout << std::string(result) << std::endl;
}

void ViveportApiDemo::MyLicenseChecker::OnFailure(int error_code, const std::string& error_message) {
    char result[256] = { '\0' };
    sprintf_s(
        result,
        "Verify failed!\n error_code=%d,\n error_message=%s",
        error_code,
        error_message.c_str()
        );
    std::cout << std::string(result) << std::endl;
}

void ViveportApiDemo::MyInitCallback::OnSuccess()
{
    std::cout << "Init success." << std::endl;
}

void ViveportApiDemo::MyInitCallback::OnFailure(int error_code)
{
    std::cout << "Init failure. error=" << error_code << std::endl;
}

void ViveportApiDemo::MyShutdownCallback::OnSuccess()
{
    std::cout << "Shutdown success." << std::endl;
}

void ViveportApiDemo::MyShutdownCallback::OnFailure(int error_code)
{
    std::cout << "Shutdown failure. error=" << error_code << std::endl;
}

void CViveportToken::IsReady(
    ViveportApiStatus *callback
    )
{
    token_is_ready_callback = callback;
    ViveportToken()->IsReady(IsTokenReadyHandler);
}

void CViveportToken::GetSessionToken(
    ViveportTokenStatus *callback)
{
    session_token_callback = callback;
    ViveportToken()->GetSessionToken(GetSessionTokenHandler);
}

void CViveportSubscription::IsReady(ViveportSubscriptionStatus *callback)
{
    subscription_is_ready_callback = callback;
    ViveportSubscription()->IsReady(IsSubscriptionReadyHandler);
}

bool CViveportSubscription::IsWindowsSubscriber()
{
    return ViveportSubscription()->IsWindowsSubscriber();
}

bool CViveportSubscription::IsAndroidSubscriber()
{
    return ViveportSubscription()->IsAndroidSubscriber();
}

SubscriptionTransactionType CViveportSubscription::GetTransactionType()
{
    switch (ViveportSubscription()->GetTransactionType())
    {
        case ESubscriptionTransactionType::UNKNOWN:
            return SubscriptionTransactionType::Unknown;
        case ESubscriptionTransactionType::PAID:
            return SubscriptionTransactionType::Paid;
        case ESubscriptionTransactionType::REDEEM:
            return SubscriptionTransactionType::Redeem;
        case ESubscriptionTransactionType::FREE_TRIAL:
            return SubscriptionTransactionType::FreeTrial;
        default:
            return SubscriptionTransactionType::Unknown;
    }
}

int CViveportDlc::IsReady(
    ViveportApiStatus *callback)
{
    dlc_is_ready_callback = callback;

    return ViveportDlc()->IsReady(IsDlcReadyHandler);
}

int CViveportDlc::GetCount()
{
    return ViveportDlc()->GetCount();
}

bool CViveportDlc::GetIsAvailable(int index, char* appId, bool &isAvailable)
{
    return ViveportDlc()->GetIsAvailable(index, appId, isAvailable);
}

int CViveportDlc::IsSubscribed()
{
    return ViveportDlc()->IsSubscribed();
}

void IsDlcReadyHandler(int nResult)
{
    return nResult == 0 ? dlc_is_ready_callback->OnSuccess() : dlc_is_ready_callback->OnFailure(nResult);
}

void IsSubscriptionReadyHandler(int nResult, const char* message)
{
    return nResult == 0 ? subscription_is_ready_callback->OnSuccess() : subscription_is_ready_callback->OnFailure(nResult, message);
}

void IsTokenReadyHandler(int nResult)
{
    return nResult == 0 ? token_is_ready_callback->OnSuccess() : token_is_ready_callback->OnFailure(nResult);
}

void GetSessionTokenHandler(
    int nResult,
    const char* message
    )
{
    return nResult == 0 ? session_token_callback->OnSuccess(message) : session_token_callback->OnFailure(nResult, message);
}

ViveportDlcDemo::ViveportDlcDemo()
{
    ViveportApi::Init(&myInitCallback, PARENT_APP_ID);
}

void ViveportDlcDemo::MyInitCallback::OnSuccess()
{
    std::cout << "Init success." << std::endl;
}

void ViveportDlcDemo::MyInitCallback::OnFailure(int error_code)
{
    std::cout << "Init failure. error=" << error_code << std::endl;
}

void ViveportDlcDemo::MyDlcIsReadyCallback::OnSuccess()
{
    std::cout << "DLC isReady success." << std::endl;
    int count = CViveportDlc::GetCount();
    char* dlcAppId = new char[37];
    bool isAvailable = false;
    CViveportDlc::GetIsAvailable(0, dlcAppId, isAvailable);
    std::cout << "DLC App ID at index 0: " << dlcAppId <<
        ", Is DLC Already purchase: " <<
        (isAvailable ? "Yes" : "No") << std::endl;
}

void ViveportDlcDemo::MyDlcIsReadyCallback::OnFailure(int error_code)
{
    std::cout << "DLC isReady failure. error=" << error_code << std::endl;
}

void ViveportDlcDemo::MyShutdownCallback::OnSuccess()
{
    std::cout << "Shutdown success." << std::endl;
}

void ViveportDlcDemo::MyShutdownCallback::OnFailure(int error_code)
{
    std::cout << "Shutdown failure. error=" << error_code << std::endl;
}

void ViveportDlcDemo::Start()
{
    CViveportDlc::IsReady(&myDlcIsReadyCallback);
}

ViveportDlcDemo::~ViveportDlcDemo()
{
    ViveportApi::Shutdown(&myShutdownCallback);
}

ViveportSubscriptionDemo::ViveportSubscriptionDemo() {}

void ViveportSubscriptionDemo::MySubscriptionIsReadyCallback::OnSuccess()
{
    std::cout << "Subscription isReady success." << std::endl;
    std::cout << "Is windows subscriber: " << (CViveportSubscription::IsWindowsSubscriber() ? "true" : "false") << std::endl;
    std::cout << "Is android subscriber: " << (CViveportSubscription::IsAndroidSubscriber() ? "true" : "false") << std::endl;
    switch (CViveportSubscription::GetTransactionType())
    {
        case SubscriptionTransactionType::Unknown:
            std::cout << "Transaction type: Unknown." << std::endl;
            break;
        case SubscriptionTransactionType::Paid:
            std::cout << "Transaction type: Paid." << std::endl;
            break;
        case SubscriptionTransactionType::Redeem:
            std::cout << "Transaction type: Redeem." << std::endl;
            break;
        case SubscriptionTransactionType::FreeTrial:
            std::cout << "Transaction type: Free Trial" << std::endl;
            break;
        default:
            std::cout << "Transaction type: Unknown." << std::endl;
            break;
    }
}

void ViveportSubscriptionDemo::MySubscriptionIsReadyCallback::OnFailure(int error_code, const char* error_message)
{
    std::cout << "Subscription isReady failure. error code=" << error_code << ", error message=" << error_message << std::endl;
}

void ViveportSubscriptionDemo::MyShutdownCallback::OnSuccess()
{
    std::cout << "Shutdown success." << std::endl;
}

void ViveportSubscriptionDemo::MyShutdownCallback::OnFailure(int error_code)
{
    std::cout << "Shutdown failure. error=" << error_code << std::endl;
}

void ViveportSubscriptionDemo::Start()
{
    CViveportSubscription::IsReady(&mySubscriptionIsReadyCallback);
}

ViveportSubscriptionDemo::~ViveportSubscriptionDemo()
{
    ViveportApi::Shutdown(&myShutdownCallback);
}

ViveportTokenDemo::ViveportTokenDemo()
{
    ViveportApi::Init(&myInitCallback, APP_ID);
}

void ViveportTokenDemo::MyInitCallback::OnSuccess()
{
    std::cout << "Init success." << std::endl;
}

void ViveportTokenDemo::MyInitCallback::OnFailure(int error_code)
{
    std::cout << "Init failure. error=" << error_code << std::endl;
}

void ViveportTokenDemo::MyTokenIsReadyCallback::OnSuccess()
{
    std::cout << "Token isReady success." << std::endl;
    CViveportToken::GetSessionToken(&g_session_token_callback);
}

void ViveportTokenDemo::MyTokenIsReadyCallback::OnFailure(int error_code)
{
    std::cout << "Token isReady failure. error=" << error_code << std::endl;
}

void ViveportTokenDemo::MySessionTokenCallback::OnSuccess(const std::string& token)
{
    std::cout << "Get session token success. token=" << token << std::endl;
}

void ViveportTokenDemo::MySessionTokenCallback::OnFailure(int error_code,
    const std::string& error_message)
{
    std::cout << "Get session token failure. error=" << error_code;
    if (!error_message.empty())
    {
        std::cout << ", error message=" << error_message;
    }
    std::cout << std::endl;
}

void ViveportTokenDemo::MyShutdownCallback::OnSuccess()
{
    std::cout << "Shutdown success." << std::endl;
}

void ViveportTokenDemo::MyShutdownCallback::OnFailure(int error_code)
{
    std::cout << "Shutdown failure. error=" << error_code << std::endl;
}

void ViveportTokenDemo::Start()
{
    g_session_token_callback = mySessionTokenCallback;
    CViveportToken::IsReady(&myTokenIsReadyCallback);
}

ViveportTokenDemo::~ViveportTokenDemo()
{
    ViveportApi::Shutdown(&myShutdownCallback);
}