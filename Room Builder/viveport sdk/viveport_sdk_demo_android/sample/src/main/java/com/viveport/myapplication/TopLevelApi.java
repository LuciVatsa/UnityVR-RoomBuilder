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
 * Use the {@link TopLevelApi#newInstance} factory method to
 * create an instance of this fragment.
 */
public class TopLevelApi extends Fragment {

    private static final String TAG = TopLevelApi.class.getSimpleName();
    private static final String APP_ID = "bd67b286-aafc-449d-8896-bb7e9b351876";

    @BindView(R.id.init)
    Button init;
    @BindView(R.id.version)
    Button version;
    @BindView(R.id.shutdown)
    Button shutdown;
    @BindView(R.id.log)
    TextView log;
    @BindView(R.id.scroll_view)
    ScrollView scrollView;

    Unbinder unbinder;

    private boolean mInit = false;

    public TopLevelApi() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @return A new instance of fragment TopLevelApi.
     */
    // TODO: Rename and change types and number of parameters
    public static TopLevelApi newInstance() {
        TopLevelApi fragment = new TopLevelApi();
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
        View view = inflater.inflate(R.layout.fragment_top_level_api, container, false);
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

    Api.StatusCallback shutdownCallback = new Api.StatusCallback() {
        @Override
        public void onResult(int statusCode, String result) {
            mInit = !(statusCode == 0);
            updateUi();
            if (statusCode == 0) {
                log2Tv("Api shutdown is successful");
            } else {
                log2Tv("Api shutdown is error, statusCode:" + statusCode + ", result:" + result);
            }
        }
    };

    @OnClick(R.id.init)
    public void init(View view) {
        Api.init(getContext(), initCallback, APP_ID);
    }

    @OnClick(R.id.version)
    public void version(View view) {
        log2Tv("Version: " + Api.version());
    }


    @OnClick(R.id.shutdown)
    public void shutdown(View view) {
        Api.shutdown(shutdownCallback);
    }

    @Override
    public void onAttach(Context context) {
        super.onAttach(context);

        ((MainActivity) getActivity()).onSectionAttached(1);
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
                version.setEnabled(mInit);
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
