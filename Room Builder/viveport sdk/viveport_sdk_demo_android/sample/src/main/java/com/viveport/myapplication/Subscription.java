package com.viveport.myapplication;

import android.app.Fragment;
import android.content.Context;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ScrollView;
import android.widget.TextView;

import com.htc.viveport.Api;

import butterknife.BindView;
import butterknife.ButterKnife;
import butterknife.OnClick;
import butterknife.Unbinder;

public class Subscription extends Fragment {

    private static final String TAG = TopLevelApi.class.getSimpleName();
    private static final String APP_ID = "ef84bf49-dea6-4070-b0d8-799dc1fb77df";

    @BindView(R.id.init)
    Button init;
    @BindView(R.id.is_ready)
    Button isReady;
    @BindView(R.id.transaction_type)
    Button transactionType;
    @BindView(R.id.is_windows_subscriber)
    Button isWindowsSubscriber;
    @BindView(R.id.is_android_subscriber)
    Button isAndroidSubscriber;
    @BindView(R.id.shutdown)
    Button shutdown;
    @BindView(R.id.log)
    TextView log;
    @BindView(R.id.scroll_view)
    ScrollView scrollView;

    Unbinder unbinder;

    private boolean mInit = false;
    private boolean mIsReady = false;

    public Subscription() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @return A new instance of fragment TopLevelApi.
     */
    // TODO: Rename and change types and number of parameters
    public static Subscription newInstance() {
        Subscription fragment = new Subscription();
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
        View view = inflater.inflate(R.layout.fragment_subscription, container, false);
        unbinder = ButterKnife.bind(this, view);
        return view;
    }

    Api.StatusCallback initCallback = new Api.StatusCallback() {
        @Override
        public void onResult(int statusCode, String result) {
            mInit = statusCode == 0;
            updateUi();
            if (statusCode == 0) {
                log2Tv("Api init is successful");
            } else {
                log2Tv("Api init is error, statusCode:" + statusCode + ", result:" + result);
            }
        }
    };

    Api.StatusCallback isReadyCallback = new Api.StatusCallback() {
        @Override
        public void onResult(int statusCode, String result) {
            mIsReady = statusCode == 0;
            updateUi();
            if (statusCode == 0) {
                log2Tv("Subscription APIs are ready.");
            } else {
                log2Tv("IsReady is error, statusCode:" + statusCode + ", result:" + result);
            }
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

    @OnClick(R.id.init)
    public void init() { Api.init(getContext(), initCallback, APP_ID); }

    @OnClick(R.id.is_ready)
    public void isReady() { com.htc.viveport.Subscription.isReady(getContext(), isReadyCallback); }

    @OnClick(R.id.transaction_type)
    public void isSubscribed() {
        log2Tv("User transaction type:" + com.htc.viveport.Subscription.getTransactionType());
    }

    @OnClick(R.id.is_windows_subscriber)
    public void isWindowsSubscriber() {
        if (com.htc.viveport.Subscription.isWindowsSubscriber()) {
            log2Tv("User is a windows subscriber");
        } else {
            log2Tv("User is not a windows subscriber");
        }
    }

    @OnClick(R.id.is_android_subscriber)
    public void isAndroidSubscriber() {
        if (com.htc.viveport.Subscription.isAndroidSubscriber()) {
            log2Tv("User is a android subscriber");
        } else {
            log2Tv("User is not a android subscriber");
        }
    }

    @OnClick(R.id.shutdown)
    public void shutdown() { Api.shutdown(shutdownCallback); }

    @Override
    public void onAttach(Context context) {
        super.onAttach(context);

        ((MainActivity) getActivity()).onSectionAttached(8);
    }

    @Override
    public void onDetach() {
        super.onDetach();
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
    public void onDestroyView() {
        super.onDestroyView();
        unbinder.unbind();
    }

    private void updateUi() {
        getActivity().runOnUiThread(new Runnable() {
            public void run() {
                init.setEnabled(!mInit);
                isReady.setEnabled(mInit);
                transactionType.setEnabled(mIsReady);
                isWindowsSubscriber.setEnabled(mIsReady);
                isAndroidSubscriber.setEnabled(mIsReady);
                shutdown.setEnabled(mInit);
            }
        });
    }

    @Override
    public void onResume() {
        super.onResume();
        updateUi();
    }
}
