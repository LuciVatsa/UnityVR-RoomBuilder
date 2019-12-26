package com.viveport.myapplication;


import android.app.Fragment;
import android.content.Context;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ScrollView;
import android.widget.TextView;

import com.htc.viveport.Api;
import com.htc.viveport.UserStats;

import butterknife.BindView;
import butterknife.ButterKnife;
import butterknife.OnClick;
import butterknife.Unbinder;


/**
 * A simple {@link Fragment} subclass.
 * Use the {@link Leaderboard#newInstance} factory method to
 * create an instance of this fragment.
 */
public class Leaderboard extends Fragment {

    private static final String TAG = Leaderboard.class.getSimpleName();
    private static final String APP_ID = "240ae821-e774-443c-b85e-f60c58aca1b7";

    @BindView(R.id.init)
    Button init;
    @BindView(R.id.is_ready)
    Button isReady;
    @BindView(R.id.shutdown)
    Button shutdown;
    @BindView(R.id.log)
    TextView log;
    @BindView(R.id.scroll_view)
    ScrollView scrollView;
    Unbinder unbinder;
    @BindView(R.id.leaderboard_name)
    EditText leaderboardName;
    @BindView(R.id.score)
    EditText score;
    @BindView(R.id.download_leaderboard)
    Button downloadLeaderboard;
    @BindView(R.id.download_leaderboard_around)
    Button downloadLeaderboardAround;
    @BindView(R.id.upload_leaderboard)
    Button uploadLeaderboard;
    @BindView(R.id.leaderboard_count)
    Button leaderboardCount;
    @BindView(R.id.leaderboard_score)
    Button leaderboardScore;
    @BindView(R.id.leaderboard_sort)
    Button leaderboardSort;
    @BindView(R.id.leaderboard_type)
    Button leaderboardType;

    // TODO: Rename parameter arguments, choose names that match

    private boolean mInit = false, mIsReady = false;

    public Leaderboard() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @return A new instance of fragment UserProfile.
     */
    // TODO: Rename and change types and number of parameters
    public static Leaderboard newInstance() {
        Leaderboard fragment = new Leaderboard();
        return fragment;
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.fragment_leaderboard, container, false);
        unbinder = ButterKnife.bind(this, view);
        return view;
    }

    @Override
    public void onDestroyView() {
        super.onDestroyView();
        unbinder.unbind();
    }

    Api.StatusCallback2 initCallback = new Api.StatusCallback2() {
        @Override
        public void onSuccess() {
            mInit = true;
            updateUi();
            log2Tv("Api init is successful");
        }

        @Override
        public void onFailure(int statusCode, String result) {
            mInit = false;
            updateUi();
            log2Tv("Api init is error, statusCode:" + statusCode + ", result:" + result);
        }
    };

    Api.StatusCallback2 shutdownCallback = new Api.StatusCallback2() {
        @Override
        public void onSuccess() {
            mInit = false;
            mIsReady = false;
            updateUi();
            log2Tv("Api shutdown is successful");
        }

        @Override
        public void onFailure(int statusCode, String result) {
            log2Tv("Api shutdown is error, statusCode:" + statusCode + ", result:" + result);
        }
    };

    Api.StatusCallback2 isReadyListener = new Api.StatusCallback2() {
        @Override
        public void onSuccess() {
            log2Tv("IsReady onSuccess");
            mIsReady = true;
            updateUi();
        }

        @Override
        public void onFailure(int i, String s) {
            log2Tv("IsReady onFailure");
            mIsReady = false;
            updateUi();
        }
    };

    Api.StatusCallback2 getLeaderboardCallback = new Api.StatusCallback2() {
        @Override
        public void onSuccess() {
            log2Tv("getLeaderboard is successful");
        }

        @Override
        public void onFailure(int i, String s) {
            log2Tv("getLeaderboard onFailure");
        }
    };

    Api.StatusCallback2 setLeaderboard = new Api.StatusCallback2() {
        @Override
        public void onSuccess() {
            log2Tv("setLeaderboard is successful");
        }

        @Override
        public void onFailure(int i, String s) {
            log2Tv("setLeaderboard onFailure");
        }
    };

    @OnClick({R.id.init, R.id.is_ready, R.id.shutdown, R.id.download_leaderboard, R.id.download_leaderboard_around, R.id.upload_leaderboard, R.id.leaderboard_count, R.id.leaderboard_score, R.id.leaderboard_sort, R.id.leaderboard_type})
    public void onViewClicked(View view) {
        switch (view.getId()) {
            case R.id.init:
                Api.init(getContext(), initCallback, APP_ID);
                break;
            case R.id.shutdown:
                Api.shutdown(shutdownCallback);
                break;
            case R.id.is_ready:
                UserStats.isReady(getContext(), isReadyListener);
                break;
            case R.id.download_leaderboard:
                UserStats.getLeaderboard(getContext(),
                        getLeaderboardCallback, leaderboardName.getText().toString(),
                        0,
                        10,
                        UserStats.ELeaderboardTimeRange.k_ELeaderboardDataScropeAllTime.ordinal(),
                        UserStats.ELeaderboardRequestType.k_ELeaderboardDataRequestGlobal.ordinal());
                break;
            case R.id.download_leaderboard_around:
                UserStats.getLeaderboard(getContext(),
                        getLeaderboardCallback, leaderboardName.getText().toString(),
                        0,
                        10,
                        UserStats.ELeaderboardTimeRange.k_ELeaderboardDataScropeAllTime.ordinal(),
                        UserStats.ELeaderboardRequestType.k_ELeaderboardDataRequestGlobalAroundUser.ordinal());
                break;
            case R.id.upload_leaderboard:
                UserStats.setLeaderboard(getContext(), setLeaderboard, leaderboardName.getText().toString(), Integer.valueOf(score.getText().toString()));
                break;
            case R.id.leaderboard_count:
                log2Tv("getLeaderboardScoreCount:" + UserStats.getLeaderboardScoreCount());
                break;
            case R.id.leaderboard_score:
                for (int i = 0; i < UserStats.getLeaderboardScoreCount(); i ++) {
                    UserStats.RankingItem rankingItem = UserStats.getLeaderboardScore(i);
                    int rank = rankingItem.rank;
                    String name = rankingItem.name;
                    int soure = rankingItem.score;
                    log2Tv("rank=" + rank +", name=" + name + ", soure=" + soure);
                }
                break;
            case R.id.leaderboard_sort:
                log2Tv("getLeaderboardSortType:" + UserStats.getLeaderboardSortType());
                break;
            case R.id.leaderboard_type:
                log2Tv("getLeaderboardDisplayType:" + UserStats.getLeaderboardDisplayType());
                break;
        }
    }

    private void updateUi() {
        getActivity().runOnUiThread(new Runnable() {
            public void run() {
                init.setEnabled(!mInit);
                isReady.setEnabled(mInit);
                shutdown.setEnabled(mIsReady);
                downloadLeaderboard.setEnabled(mIsReady);
                downloadLeaderboardAround.setEnabled(mIsReady);
                uploadLeaderboard.setEnabled(mIsReady);
                leaderboardCount.setEnabled(mIsReady);
                leaderboardScore.setEnabled(mIsReady);
                leaderboardSort.setEnabled(mIsReady);
                leaderboardType.setEnabled(mIsReady);
            }
        });
    }

    private void log2Tv(final String msg) {
        getActivity().runOnUiThread(new Runnable() {
            public void run() {
                if (log != null) log.setText(msg + "\n" + log.getText().toString());
                if (scrollView != null) scrollView.fullScroll(ScrollView.FOCUS_UP);
            }
        });
        Log.d(TAG, msg);
    }

    @Override
    public void onResume() {
        super.onResume();
        updateUi();
    }

    @Override
    public void onAttach(Context context) {
        super.onAttach(context);
        ((MainActivity) getActivity()).onSectionAttached(3);
    }
}
