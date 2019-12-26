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
import com.htc.viveport.User;
import com.htc.viveport.UserStats;

import butterknife.BindView;
import butterknife.ButterKnife;
import butterknife.OnClick;
import butterknife.Unbinder;


/**
 * A simple {@link Fragment} subclass.
 * Use the {@link UserProfile#newInstance} factory method to
 * create an instance of this fragment.
 */
public class UserProfile extends Fragment {

    private static final String TAG = UserProfile.class.getSimpleName();
    private static final String APP_ID = "bd67b286-aafc-449d-8896-bb7e9b351876";

    @BindView(R.id.init)
    Button init;
    @BindView(R.id.is_ready)
    Button isReady;
    @BindView(R.id.user_id)
    Button userId;
    @BindView(R.id.user_name)
    Button userName;
    @BindView(R.id.user_avatar)
    Button userAvatar;
    @BindView(R.id.shutdown)
    Button shutdown;
    @BindView(R.id.log)
    TextView log;
    @BindView(R.id.scroll_view)
    ScrollView scrollView;
    Unbinder unbinder;
    // TODO: Rename parameter arguments, choose names that match

    private boolean mInit = false, mIsReady = false;

    public UserProfile() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @return A new instance of fragment UserProfile.
     */
    // TODO: Rename and change types and number of parameters
    public static UserProfile newInstance() {
        UserProfile fragment = new UserProfile();
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
        View view = inflater.inflate(R.layout.fragment_user_profile, container, false);
        unbinder = ButterKnife.bind(this, view);
        return view;
    }

    @Override
    public void onDestroyView() {
        super.onDestroyView();
        unbinder.unbind();
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
            log2Tv("UserProfile isReady is successful");
            mIsReady = true;
            updateUi();
        }

        @Override
        public void onFailure(int i, String s) {
            log2Tv("UserProfile onFailure");
            mIsReady = false;
            updateUi();
        }
    };

    @OnClick({R.id.init, R.id.is_ready, R.id.user_id, R.id.user_name, R.id.user_avatar, R.id.shutdown})
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
            case R.id.user_id:
                log2Tv("UserId: " + User.getUserID(getContext()));
                break;
            case R.id.user_name:
                log2Tv("UserName: " + User.getUserName(getContext()));
                break;
            case R.id.user_avatar:
                log2Tv("UserAvatar: " + User.getUserAvatar(getContext()));
                break;
        }
    }

    private void updateUi() {
        getActivity().runOnUiThread(new Runnable() {
            public void run() {
                init.setEnabled(!mInit);
                isReady.setEnabled(mInit);
                shutdown.setEnabled(mInit);
                userId.setEnabled(mIsReady);
                userName.setEnabled(mIsReady);
                userAvatar.setEnabled(mIsReady);
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
