#pragma once

#include <string>

typedef void(*ViveportSDKStatusCallback)(int nResult);

class ViveportApiStatus
{
public:
    virtual ~ViveportApiStatus() { }
    virtual void OnSuccess() = 0;
    virtual void OnFailure(int nError) = 0;

protected:
    ViveportApiStatus() { }
};

class ViveportApi
{
public:
    class LicenseChecker;

    static int Init(
        ViveportApiStatus *callback,
        const std::string app_id
        );
    static void GetLicense(
        LicenseChecker *license_checker,
        const std::string app_id,
        const std::string app_key
        );
    static int Shutdown(ViveportApiStatus *callback);
    static std::string Version();
};

class ViveportApi::LicenseChecker
{
public:
    virtual ~LicenseChecker() { }
    virtual void OnSuccess(
        long long issue_time,
        long long expiration_time,
        int latest_version,
        bool update_required
        ) = 0;
    virtual void OnFailure(
        int errorCode,
        const std::string& error_message
        ) = 0;

protected:
    LicenseChecker() { }
};

class ViveportApiDemo
{
public:
    // Sets default values for this component's properties
    ViveportApiDemo();
    ~ViveportApiDemo();
    void Start();
    /** The APP ID for auth verify */
    std::string APP_ID = "bd67b286-aafc-449d-8896-bb7e9b351876";

    /** Public key for auth verify */
    std::string APP_KEY = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDFypCg0OHfBC+VZLSWPbNSgDo9qg/yQORDwGy1rKIboMj3IXn4Zy6h6bgn8kiMY7VI0lPwIj9lijT3ZxkzuTsI5GsK//Y1bqeTol4OUFR+47gj+TUuekAS2WMtglKox+/7mO6CA1gV+jZrAKo6YSVmPd+oFsgisRcqEgNh5MIURQIDAQAB";

private:
    // callback interface
    class MyLicenseChecker : public ViveportApi::LicenseChecker
    {
    public:
        void OnSuccess(
            long long issue_time,
            long long expiration_time,
            int latest_version,
            bool update_required
            ) override;
        void OnFailure(
            int errorCode,
            const std::string& errorMessage
            ) override;
    };

    MyLicenseChecker myLicenseChecker;

    class MyInitCallback : public ViveportApiStatus
    {
    public:
        void OnSuccess(
            ) override;
        void OnFailure(int error_code
            ) override;
    };

    MyInitCallback myInitCallback;

    class MyShutdownCallback : public ViveportApiStatus
    {
    public:
        void OnSuccess(
            ) override;
        void OnFailure(int error_code
            ) override;
    };

    MyShutdownCallback myShutdownCallback;
};

class ViveportTokenStatus
{
public:
    virtual ~ViveportTokenStatus() { }
    virtual void OnSuccess(const std::string& token) = 0;
    virtual void OnFailure(int nError, const std::string& errorMessage) = 0;

protected:
    ViveportTokenStatus() { }
};

class CViveportToken
{
public:
    static void IsReady(
        ViveportApiStatus *callback
        );
    static void GetSessionToken(
        ViveportTokenStatus *callback
        );
};

enum SubscriptionTransactionType
{
    Unknown,
    Paid,
    Redeem,
    FreeTrial
};

class ViveportSubscriptionStatus
{
public:
    virtual ~ViveportSubscriptionStatus() {}
    virtual void OnSuccess() = 0;
    virtual void OnFailure(int nError, const char* errorMessage) = 0;
};

class CViveportSubscription
{
public:
    static void IsReady(ViveportSubscriptionStatus *callback);
    static bool IsWindowsSubscriber();
    static bool IsAndroidSubscriber();
    static SubscriptionTransactionType GetTransactionType();
};

class CViveportDlc
{
public:
    static int IsReady(
        ViveportApiStatus *callback
        );
    static int GetCount();
    static bool GetIsAvailable(int index, char* appId, bool &isAvailable);
    static int IsSubscribed();
};

class ViveportTokenDemo
{
public:
    ViveportTokenDemo();
    ~ViveportTokenDemo();
    void Start();
    std::string APP_ID = "bd67b286-aafc-449d-8896-bb7e9b351876";

    class MySessionTokenCallback : public ViveportTokenStatus
    {
    public:
        void OnSuccess(const std::string& token
            ) override;
        void OnFailure(int error_code,
            const std::string& error_message
            ) override;
    };

private:
    MySessionTokenCallback mySessionTokenCallback;

    // callback interface
    class MyInitCallback : public ViveportApiStatus
    {
    public:
        void OnSuccess(
            ) override;
        void OnFailure(int error_code
            ) override;
    };

    MyInitCallback myInitCallback;

    class MyTokenIsReadyCallback : public ViveportApiStatus
    {
    public:
        void OnSuccess(
            ) override;
        void OnFailure(int error_code
            ) override;
    };

    MyTokenIsReadyCallback myTokenIsReadyCallback;

    class MyShutdownCallback : public ViveportApiStatus
    {
    public:
        void OnSuccess(
            ) override;
        void OnFailure(int error_code
            ) override;
    };

    MyShutdownCallback myShutdownCallback;
};

class ViveportDlcDemo
{
public:
    ViveportDlcDemo();
    ~ViveportDlcDemo();

    void Start();
    std::string PARENT_APP_ID = "76d0898e-8772-49a9-aa55-1ec251a21686";

    // callback interface
    class MyInitCallback : public ViveportApiStatus
    {
    public:
        void OnSuccess(
        ) override;
        void OnFailure(int error_code
        ) override;
    };

    MyInitCallback myInitCallback;

    class MyDlcIsReadyCallback : public ViveportApiStatus
    {
    public:
        void OnSuccess(
        ) override;
        void OnFailure(int error_code
        ) override;
    };

    MyDlcIsReadyCallback myDlcIsReadyCallback;

    class MyShutdownCallback : public ViveportApiStatus
    {
    public:
        void OnSuccess(
        ) override;
        void OnFailure(int error_code
        ) override;
    };

    MyShutdownCallback myShutdownCallback;
};

class ViveportSubscriptionDemo
{
public:
    ViveportSubscriptionDemo();
    ~ViveportSubscriptionDemo();

    void Start();
    std::string APP_ID = "ef84bf49-dea6-4070-b0d8-799dc1fb77df";

    class MySubscriptionIsReadyCallback : public ViveportSubscriptionStatus
    {
    public:
        void OnSuccess(
        ) override;
        void OnFailure(int error_code, const char* error_message
        ) override;
    };

    MySubscriptionIsReadyCallback mySubscriptionIsReadyCallback;

    class MyShutdownCallback : public ViveportApiStatus
    {
    public:
        void OnSuccess(
        ) override;
        void OnFailure(int error_code
        ) override;
    };

    MyShutdownCallback myShutdownCallback;
};