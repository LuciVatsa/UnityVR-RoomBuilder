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
 * Use the {@link Stats#newInstance} factory method to
 * create an instance of this fragment.
 */
public class Stats extends Fragment {

    private static final String TAG = Stats.class.getSimpleName();
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
    @BindView(R.id.download_stats)
    Button downloadStats;
    @BindView(R.id.upload_stats)
    Button uploadStats;
    @BindView(R.id.stat_key)
    EditText statKey;
    @BindView(R.id.stat_value)
    EditText statValue;
    @BindView(R.id.get_stat)
    Button getStat;
    @BindView(R.id.set_stat)
    Button setStat;
    // TODO: Rename parameter arguments, choose names that match

    private boolean mInit = false, mIsReady = false;

    public Stats() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @return A new instance of fragment UserProfile.
     */
    // TODO: Rename and change types and number of parameters
    public static Stats newInstance() {
        Stats fragment = new Stats();
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
        View view = inflater.inflate(R.layout.fragment_stats, container, false);
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

    @OnClick({R.id.init, R.id.is_ready, R.id.shutdown, R.id.download_stats, R.id.upload_stats, R.id.get_stat, R.id.set_stat})
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
            case R.id.get_stat:
                int value = UserStats.getStats(statKey.getText().toString(), 0);
                statValue.setText(String.valueOf(value));
                break;
            case R.id.set_stat:
                UserStats.setStats(statKey.getText().toString(), Integer.valueOf(statValue.getText().toString()));
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
                getStat.setEnabled(mIsReady);
                setStat.setEnabled(mIsReady);
                statKey.setEnabled(mIsReady);
                statValue.setEnabled(mIsReady);
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
