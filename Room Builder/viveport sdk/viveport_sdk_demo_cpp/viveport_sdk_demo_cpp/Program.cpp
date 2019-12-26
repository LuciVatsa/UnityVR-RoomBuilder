// ConsoleApplication2.cpp : Defines the entry point for the console application.
//
#include <string>
#include <iostream>
#include <mutex>
#include <condition_variable>

#include "viveport_api.h"
#include "VIVEPORT.h"
#define SMALL_BUF_SIZE 3

using namespace std;

int showMenu(void);
void testGetUserId(void);
void testGetUserName(void);
void testGetUserAvatarUrl(void);
void testStatAPI(void);
void testUploadStatAPI(void);
void testDownloadLeaderboard(void);
void testUploadLeaderboard(void);
void testGetLicense(void);
void testGetSessionToken(void);
void testGetUserDlcApps(void);
void testSubscription(void);

const string STAT1 = "ID_Stat1";
const string ACHIE1 = "ID_Achievement1";
const string LEADERBOARD1 = "ID_Leaderboard1";
const string APP_ID = "bd67b286-aafc-449d-8896-bb7e9b351876";
const string APP_KEY = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDFypCg0OHf\
                        BC+VZLSWPbNSgDo9qg/yQORDwGy1rKIboMj3IXn4Zy6h6bgn\
                        8kiMY7VI0lPwIj9lijT3ZxkzuTsI5GsK//Y1bqeTol4OUFR+\
                        47gj+TUuekAS2WMtglKox+/7mO6CA1gV+jZrAKo6YSVmPd+o\
                        FsgisRcqEgNh5MIURQIDAQAB";
const string PARENT_APP_ID = "76d0898e-8772-49a9-aa55-1ec251a21686";

#define WAIT_TIMEOUT 10
static std::mutex m, mIAP;
static std::condition_variable cv, cvIAP;
static bool processed = false, processedIAP = false;

bool apiInit = false;
bool isStatApiReady = false;
bool isIAPApiReady = false;
bool isStatDataReady = false;

ViveportApiDemo viveportApiDemo;
ViveportTokenDemo viveportTokenDemo;
ViveportDlcDemo viveportDlcDemo;
ViveportSubscriptionDemo viveportSubscriptionDemo;

void TestIsStatReadyCallback(int nResult)
{
    printf("[TestIsStatReadyCallback] result: %d\n", nResult);
    processed = true;
    isStatApiReady = nResult == 0;
    cv.notify_one();
}

void TestViveportIAPReadyCallback(int code, const char* message)
{
    printf("[TestViveportIAPReadyCallback] result: %d, message: %s", code, message);
    processedIAP = true;
    isIAPApiReady = code == 0;
    cvIAP.notify_one();
}

void TestViveportAPIInitCallback(int nResult)
{
    apiInit = nResult == 0;
    if (apiInit) {
        ViveportUserStats()->IsReady(TestIsStatReadyCallback);
        ViveportIAPurchase()->IsReady(TestViveportIAPReadyCallback, APP_KEY.c_str());
    }
}

static void waitCallback() {
    std::unique_lock<std::mutex> lk(m);
    bool isNotTimeout = cv.wait_for(lk, chrono::seconds(WAIT_TIMEOUT), [] {return processed; });
    if (isNotTimeout == false) {
        cout << "TIMEOUT!!\n";
    }
}

static void waitIAPCallback() {
    std::unique_lock<std::mutex> lk(mIAP);
    bool isNotTimeout = cvIAP.wait_for(lk, chrono::seconds(WAIT_TIMEOUT), [] {return processedIAP; });
    if (isNotTimeout == false) {
        cout << "TIMEOUT!!\n";
    }
}

int main()
{
    ViveportAPI()->Init(TestViveportAPIInitCallback, APP_ID.c_str());
    waitCallback();
    waitIAPCallback();
    showMenu();
    printf("\nplease press enter again to leave...\n");
    getchar();
    return 0;
}

void displayMenuItems() {
    printf("Main menu(api ver:%s)\n", ViveportAPI()->Version());
    printf("1.Get License\n");
    printf("2.Get Session Token\n");
    printf("3.Get User Id\n");
    printf("4.Get User Name\n");
    printf("5.Get User Avatar\n");
    printf("6.Download Stat\n");
    printf("7.Upload Stat\n");
    printf("8.Download Leaderboard\n");
    printf("9.Upload Leaderboard\n");
    printf("d.Get User DLC Apps\n");
    printf("s.Subscription\n");
    printf("q.Quit test program\n");
    printf("please enter something:");
}

int showMenu() {
    int input;

    /*Displays the menu to user*/
    displayMenuItems();
    do {
        input = getchar();
        getchar();
        switch (input) {
        case '1':
            testGetLicense();
            break;
        case '2':
            testGetSessionToken();
            break;
        case '3':
            testGetUserId();
            break;
        case '4':
            testGetUserName();
            break;
        case '5':
            testGetUserAvatarUrl();
            break;
        case '6':
            testStatAPI();
            break;
        case '7':
            testUploadStatAPI();
            break;
        case '8':
            testDownloadLeaderboard();
            break;
        case '9':
            testUploadLeaderboard();
            break;
        case 'd':
            testGetUserDlcApps();
            break;
        case 's':
            testSubscription();
            break;
        case 'q':
            break;
        default:
            printf("invalid\n");
            displayMenuItems();
            break;
        }

    } while (input != 'q');

    return EXIT_SUCCESS;
}

void TestViveportAPIGetLicenseCallback(const char* message, const char* signature)
{
    printf("invalid\n");
    std::cout << "[TestViveportAPIGetLicenseCallback] message:[" << message << "]" << signature;
}

void testGetLicense() {
    viveportApiDemo.Start();
}

void testGetSessionToken() {
    viveportTokenDemo.Start();
}

void testGetUserDlcApps()
{
    ViveportAPI()->Init(TestViveportAPIInitCallback, PARENT_APP_ID.c_str());
    waitCallback();
    viveportDlcDemo.Start();
}

void testSubscription()
{
    ViveportAPI()->Init(TestViveportAPIInitCallback, viveportSubscriptionDemo.APP_ID.c_str());
    waitCallback();
    viveportSubscriptionDemo.Start();
}

void testGetUserId()
{
    char smallbuf[SMALL_BUF_SIZE];
    int ret = 0;
    
    ret = ViveportUser()->GetUserID(smallbuf, SMALL_BUF_SIZE);

    if (ret > 0) {
        char* data = new char[ret + 1];
        ret = ViveportUser()->GetUserID(data, ret + 1);
        int len = strlen(data);

        string val(data);
        std::cout << "[ViveportUserUnitTest] testGetUserId:[" << val << "]" << std::endl;

        delete[] data;

        //ASSERT_EQ(ret, len);
        //ASSERT_STRNE(val.c_str(), "UnknownU-serI-dUnk-nown-UserIdUnknow");
    }
    else if (ret == 0) {
        std::cout << "[ViveportUserUnitTest] testGetUserId: NO UserID" << std::endl;
    }
}

void testGetUserName()
{
    char smallbuf[SMALL_BUF_SIZE];
    int ret = 0;

    ret = ViveportUser()->GetUserName(smallbuf, SMALL_BUF_SIZE);

    if (ret > 0) {
        char* data = new char[ret + 1];
        ret = ViveportUser()->GetUserName(data, ret + 1);
        int len = strlen(data);

        string val(data);
        std::cout << "[ViveportUserUnitTest] testGetUserName:[" << val << "]" << std::endl;

        delete[] data;
    }
    else if (ret == 0) {
        std::cout << "[ViveportUserUnitTest] testGetUserId: NO UserName" << std::endl;
    }
}

void testGetUserAvatarUrl()
{
    char smallbuf[SMALL_BUF_SIZE];
    int ret = 0;

    ret = ViveportUser()->GetUserAvatarUrl(smallbuf, SMALL_BUF_SIZE);

    if (ret > 0) {
        char* data = new char[ret + 1];
        ret = ViveportUser()->GetUserAvatarUrl(data, ret + 1);
        int len = strlen(data);

        string val(data);
        std::cout << "[ViveportUserUnitTest] testGetUserAvatarUrl:[" << val << "]" << std::endl;

        delete[] data;
    }
    else if (ret == 0) {
        std::cout << "[ViveportUserUnitTest] testGetUserId: NO AvatarUrl" << std::endl;
    }
}



void TestDownloadStatsCallback(int nResult) {
    printf("TestDownloadStatsCallback: nResult=%d\n", nResult);
    isStatDataReady = nResult == 0;
    int statResult = 0;
    int achResult = 0;
    int achUnlockTime = 0;
    ViveportUserStats()->GetStat(STAT1.c_str(), &statResult);
    printf("ViveportUser()->GetStat(): Stat Name: %s, result: %d\n", STAT1.c_str(), statResult);
    ViveportUserStats()->GetAchievement(ACHIE1.c_str(), &achResult);
    printf("ViveportUserStats()->GetAchievement(): Achievement Name: %s, result: %d\n", ACHIE1.c_str(), achResult);
    if (achResult == 1) {
        if (ViveportUserStats()->GetAchievementUnlockTime(ACHIE1.c_str(), &achUnlockTime) == 1)
            printf("ViveportUserStats()->GetAchievementUnlockTime(): AchKey: %s, time_sec: %d\n", ACHIE1.c_str(), achUnlockTime);
    }

}

void testStatAPI() {
    ViveportUserStats()->DownloadStats(TestDownloadStatsCallback);
}

void TestUploadStatsCallback(int nResult)
{
    printf("[TestUploadStatsCallback] result: %d\n", nResult);
}

void testUploadStatAPI() {
    int statResult = 0;
    int achResult = 0;

    if (!isStatDataReady) {
        printf("Please download stat first.");
        return;
    }
    ViveportUserStats()->GetStat(STAT1.c_str(), &statResult);
    ViveportUserStats()->GetAchievement(ACHIE1.c_str(), &achResult);
    statResult++;
    ViveportUserStats()->SetStat(STAT1.c_str(), statResult);
    if (achResult == 1) {
        ViveportUserStats()->ClearAchievement(ACHIE1.c_str());
    }
    else {
        ViveportUserStats()->SetAchievement(ACHIE1.c_str());
    }
    ViveportUserStats()->UploadStats(TestUploadStatsCallback);
}

void TestDownloadLeaderBoardCallback(int nResult)
{
    printf("[TestDownloadLeaderBoardCallback] result: %d\n", nResult);

    printf("ViveportUserStats()->GetLeaderboardScoreCount(): %d\n", ViveportUserStats()->GetLeaderboardScoreCount());
    printf("ViveportUserStats()->GetLeaderboardSortMethod(): %d\n", ViveportUserStats()->GetLeaderboardSortMethod());
    printf("ViveportUserStats()->GetLeaderboardDisplayType(): %d\n", ViveportUserStats()->GetLeaderboardDisplayType());
    for (int i = 0; i < ViveportUserStats()->GetLeaderboardScoreCount(); ++i) {
        LeaderboardScore * score = (LeaderboardScore*)malloc(sizeof(LeaderboardScore));
        ViveportUserStats()->GetLeaderboardScore(i, score);
        printf("score->UserName: %s\n", score->UserName);
        printf("score->Score: %d\n", score->Score);
        printf("score->Rank: %d\n", score->Rank);
        free(score);
    }
}

void testDownloadLeaderboard() {
    ViveportUserStats()->DownloadLeaderboardScores(TestDownloadLeaderBoardCallback, LEADERBOARD1.c_str(), k_ELeaderboardDataDownloadGlobal, k_ELeaderboardDataScropeAllTime, 0, 10);
}

void TestUploadLeaderBoardCallback(int nResult)
{
    printf("[TestUploadLeaderBoardCallback] result: %d\n", nResult);
}

void testUploadLeaderboard() {
    ViveportUserStats()->UploadLeaderboardScore(TestUploadLeaderBoardCallback, LEADERBOARD1.c_str(), 1);
}