package com.viveport.myapplication;

import android.app.Fragment;
import android.content.Context;
import android.os.Bundle;
import android.os.Handler;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
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
 * Activities that contain this fragment must implement the
 * {@link OnFragmentInteractionListener} interface
 * to handle interaction events.
 * Use the {@link DRM#newInstance} factory method to
 * create an instance of this fragment.
 */
public class DRM extends Fragment {

    private static final String TAG = DRM.class.getSimpleName();
    private static final String APP_ID = "bd67b286-aafc-449d-8896-bb7e9b351876";
    private static final String APP_KEY = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDFypCg0OHf"
            + "BC+VZLSWPbNSgDo9qg/yQORDwGy1rKIboMj3IXn4Zy6h6bgn"
            + "8kiMY7VI0lPwIj9lijT3ZxkzuTsI5GsK//Y1bqeTol4OUFR+"
            + "47gj+TUuekAS2WMtglKox+/7mO6CA1gV+jZrAKo6YSVmPd+o"
            + "FsgisRcqEgNh5MIURQIDAQAB";

    @BindView(R.id.drm)
    Button drm;
    Unbinder unbinder;
    @BindView(R.id.log)
    TextView log;
    @BindView(R.id.scroll_view)
    ScrollView scrollView;
    @BindView(R.id.init)
    Button init;
    @BindView(R.id.shutdown)
    Button shutdown;
    @BindView(R.id.is_ready)
    Button isReady;

    private boolean mInit = false, mIsReady = false;

    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER

    public DRM() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @return A new instance of fragment DRM.
     */
    // TODO: Rename and change types and number of parameters
    public static DRM newInstance() {
        DRM fragment = new DRM();
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
        View view = inflater.inflate(R.layout.fragment_drm, container, false);
        unbinder = ButterKnife.bind(this, view);
        return view;
    }

    @Override
    public void onAttach(Context context) {
        super.onAttach(context);
        ((MainActivity) getActivity()).onSectionAttached(2);
    }

    @Override
    public void onDetach() {
        super.onDetach();
    }

    @Override
    public void onDestroyView() {
        super.onDestroyView();
        unbinder.unbind();
    }

    Api.LicenseChecker licenseChecker = new Api.LicenseChecker() {
        @Override
        public void onSuccess(long issueTime,
                              long expirationTime,
                              int latestVersion,
                              boolean updateRequired) {
            log2Tv("[MyLicenseChecker] issueTime: " + issueTime);
            log2Tv("[MyLicenseChecker] expirationTime: " + expirationTime);
            log2Tv("[MyLicenseChecker] latestVersion: " + latestVersion);
            log2Tv("[MyLicenseChecker] updateRequired: " + updateRequired);
        }

        @Override
        public void onFailure(int errorCode, String errorMessage) {
            log2Tv("[MyLicenseChecker] errorCode: " + errorCode);
            log2Tv("[MyLicenseChecker] errorMessage: " + errorMessage);
        }
    };

    @OnClick({R.id.init, R.id.drm, R.id.shutdown, R.id.is_ready})
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
            case R.id.drm:
                Api.getLicense(getContext(), licenseChecker, APP_ID, APP_KEY);
                break;
        }
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

    Api.StatusCallback shutdownCallback1 = new Api.StatusCallback() {
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
            updateUi();
            log2Tv("Api shutdown is error, statusCode:" + statusCode + ", result:" + result);
        }
    };


    Api.StatusCallback2 isReadyListener = new Api.StatusCallback2() {
        @Override
        public void onSuccess() {
            log2Tv("IsReadyListener onSuccess");
            mIsReady = true;
            updateUi();
        }

        @Override
        public void onFailure(int i, String s) {
            log2Tv("IsReadyListener onFailure");
            mIsReady = false;
            updateUi();
        }
    };

    private void updateUi() {
        getActivity().runOnUiThread(new Runnable() {
            public void run() {
                init.setEnabled(!mInit);
                drm.setEnabled(mIsReady);
                isReady.setEnabled(mInit);
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
