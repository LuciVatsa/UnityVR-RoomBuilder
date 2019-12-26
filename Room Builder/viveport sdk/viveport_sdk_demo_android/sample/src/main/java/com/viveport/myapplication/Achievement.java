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
import android.widget.ImageView;
import android.widget.ScrollView;
import android.widget.TextView;

import com.bumptech.glide.Glide;
import com.htc.viveport.Api;
import com.htc.viveport.User;
import com.htc.viveport.UserStats;

import butterknife.BindView;
import butterknife.ButterKnife;
import butterknife.OnClick;
import butterknife.Unbinder;


/**
 * A simple {@link Fragment} subclass.
 * Use the {@link Achievement#newInstance} factory method to
 * create an instance of this fragment.
 */
public class Achievement extends Fragment {

    private static final String TAG = Achievement.class.getSimpleName();
    private static final String APP_ID = "d4cea7c0-6afa-438c-a740-49d2287a2b68";

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
    @BindView(R.id.download_stats)
    Button downloadStats;
    @BindView(R.id.upload_stats)
    Button uploadStats;
    @BindView(R.id.achievement_name)
    EditText achievementName;
    @BindView(R.id.achievement_value)
    EditText achievementValue;
    @BindView(R.id.get_achievement)
    Button getAchievement;
    @BindView(R.id.set_achievement)
    Button setAchievement;
    @BindView(R.id.clear_achievement)
    Button clearAchievement;
    @BindView(R.id.get_achievement_name)
    Button getAchievementName;
    @BindView(R.id.get_achievement_description)
    Button getAchievementDescription;
    @BindView(R.id.get_achievement_icon)
    Button getAchievementIcon;
    @BindView(R.id.icon)
    ImageView icon;
    // TODO: Rename parameter arguments, choose names that match

    private boolean mInit = false, mIsReady = false;

    public Achievement() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @return A new instance of fragment UserProfile.
     */
    // TODO: Rename and change types and number of parameters
    public static Achievement newInstance() {
        Achievement fragment = new Achievement();
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
        View view = inflater.inflate(R.layout.fragment_achievement, container, false);
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

    Api.StatusCallback shutdownCallback = new Api.StatusCallback() {
        @Override
        public void onResult(int statusCode, String result) {
            mInit = !(statusCode == 0);
            mIsReady = !(statusCode == 0);
            updateUi();
            if (statusCode == 0) {
                log2Tv("Api shutdown is successful");
            } else {
                log2Tv("Api shutdown is error, statusCode:" + statusCode + ", result:" + result);
            }
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

    Api.StatusCallback2 DownloadStatsListener = new Api.StatusCallback2() {
        @Override
        public void onSuccess() {
            log2Tv("DownloadStats is successful");
        }

        @Override
        public void onFailure(int i, String s) {
            log2Tv("DownloadStats onFailure");
        }
    };

    Api.StatusCallback2 UploadStatsListener = new Api.StatusCallback2() {
        @Override
        public void onSuccess() {
            log2Tv("UploadStats is successful");
        }

        @Override
        public void onFailure(int i, String s) {
            log2Tv("UploadStats onFailure");
        }
    };

    @OnClick({R.id.init, R.id.is_ready, R.id.shutdown, R.id.download_stats, R.id.upload_stats,
            R.id.get_achievement, R.id.set_achievement, R.id.clear_achievement,
            R.id.get_achievement_name, R.id.get_achievement_description, R.id.get_achievement_icon})
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
            case R.id.download_stats:
                UserStats.downloadStats(getContext(), DownloadStatsListener);
                break;
            case R.id.upload_stats:
                UserStats.uploadStats(getContext(), UploadStatsListener);
                break;
            case R.id.get_achievement:
                boolean value = UserStats.getAchievement(achievementName.getText().toString());
                achievementValue.setText(String.valueOf(value));
                break;
            case R.id.set_achievement:
                UserStats.setAchievement(achievementName.getText().toString());
                break;
            case R.id.clear_achievement:
                UserStats.clearAchievement(achievementName.getText().toString());
                break;
            case R.id.get_achievement_name:
                log2Tv(UserStats.getAchievementDisplayAttribute(achievementName.getText().toString(), UserStats.AchievementDisplayAttribute.NAME.ordinal()));
                break;
            case R.id.get_achievement_description:
                log2Tv(UserStats.getAchievementDisplayAttribute(achievementName.getText().toString(), UserStats.AchievementDisplayAttribute.DESC.ordinal()));
                break;
            case R.id.get_achievement_icon:
                String url = UserStats.getAchievementIcon(achievementName.getText().toString());
                log2Tv("achievement Icon url:" + url);
                Glide.with(this.getActivity()).load(url).into(icon);//display the achievement icon in screen
                break;
        }
    }

    private void updateUi() {
        getActivity().runOnUiThread(new Runnable() {
            public void run() {
                init.setEnabled(!mInit);
                isReady.setEnabled(mInit);
                shutdown.setEnabled(mInit);
                downloadStats.setEnabled(mIsReady);
                uploadStats.setEnabled(mIsReady);
                getAchievement.setEnabled(mIsReady);
                setAchievement.setEnabled(mIsReady);
                clearAchievement.setEnabled(mIsReady);
                getAchievementName.setEnabled(mIsReady);
                getAchievementDescription.setEnabled(mIsReady);
                getAchievementIcon.setEnabled(mIsReady);
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
