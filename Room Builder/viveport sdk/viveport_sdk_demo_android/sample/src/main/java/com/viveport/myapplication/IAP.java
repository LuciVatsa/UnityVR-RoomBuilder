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
import com.htc.viveport.IAPurchase;

import java.util.List;

import butterknife.BindView;
import butterknife.ButterKnife;
import butterknife.OnClick;
import butterknife.Unbinder;


/**
 * A simple {@link Fragment} subclass.
 * Use the {@link IAP#newInstance} factory method to
 * create an instance of this fragment.
 */
public class IAP extends Fragment {

    private static final String TAG = IAP.class.getSimpleName();
    private static final String APP_ID = "aaa47a01-0ef8-49e7-a278-5a437f9e81fc";
    private static final String APP_KEY = "R8oyULiURTS_RW6D4MpeFakqHmKh_YZ8";
//    private static final String APP_ID = "cfa07278-b859-4c80-8b2a-62e283d7be97"; //"e084bec3-74fb-4128-bda0-13feec9a9ed4"; //iap_test_02@yopmail.com/Aa0110test
//    private static final String APP_KEY = "Gmt7G5R_QPSqzF3oEI39shXBCSvQCnb7";

    Unbinder unbinder;
    @BindView(R.id.init)
    Button init;
    @BindView(R.id.is_ready)
    Button isReady;
    @BindView(R.id.get_balance)
    Button getBalance;
    @BindView(R.id.request)
    Button request;
    @BindView(R.id.request_userdata)
    Button requestUserdata;
    @BindView(R.id.purchase)
    Button purchase;
    @BindView(R.id.query)
    Button query;
    @BindView(R.id.query_purchase_list)
    Button queryPurchaseList;
    @BindView(R.id.shutdown)
    Button shutdown;
    @BindView(R.id.log)
    TextView log;
    @BindView(R.id.scroll_view)
    ScrollView scrollView;

    private boolean mInit = false;
    private boolean mIsReady = false;
    private String mPurchaseId = "", subscriptionId = "";

    public IAP() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @return A new instance of fragment IAP.
     */
    // TODO: Rename and change types and number of parameters
    public static IAP newInstance() {
        IAP fragment = new IAP();
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
        View view = inflater.inflate(R.layout.fragment_iap, container, false);
        unbinder = ButterKnife.bind(this, view);
        return view;
    }

    @Override
    public void onDestroyView() {
        super.onDestroyView();
        unbinder.unbind();
    }

    Api.StatusCallback2 initListener = new Api.StatusCallback2() {
        @Override
        public void onSuccess() {
            mInit = true;
            updateUi();
            log2Tv("Api init is successful");
        }

        @Override
        public void onFailure(int statusCode, String errorMessage) {
            mInit = false;
            updateUi();
            log2Tv("Api init is error, statusCode:" + statusCode + ", errorMessage:" + errorMessage);
        }
    };

    Api.StatusCallback2 shutdownListener = new Api.StatusCallback2() {
        @Override
        public void onSuccess() {
            mInit = false;
            mIsReady = false;
            updateUi();
            log2Tv("Api shutdown is successful");
        }

        @Override
        public void onFailure(int statusCode, String errorMessage) {
            mInit = false;
            mIsReady = false;
            updateUi();
            log2Tv("Api shutdown is error, statusCode:" + statusCode + ", errorMessage:" + errorMessage);
        }
    };

    IAPurchase.IsReadyListener isReadyListener = new IAPurchase.IsReadyListener() {
        @Override
        public void onSuccess(String currencyName) {
            mIsReady = true;
            updateUi();
            log2Tv("[onSuccess] currencyName=" + currencyName);
        }

        @Override
        public void onFailure(int statusCode, String errorMessage) {
            mIsReady = false;
            updateUi();
            log2Tv("[onFailure] statusCode=" + statusCode + ", errorMessage=" + errorMessage);
        }
    };

    IAPurchase.RequestListener requestListener = new IAPurchase.RequestListener() {
        @Override
        public void onRequestSuccess(String purchaseId) {
            mPurchaseId = purchaseId;
            log2Tv("[onRequestSuccess] purchaseId=" + purchaseId);
        }

        @Override
        public void onFailure(int statusCode, String errorMessage) {
            log2Tv("[onFailure] statusCode=" + statusCode + ", errorMessage=" + errorMessage);
        }
    };

    IAPurchase.GetBalanceListener getBalanceListener = new IAPurchase.GetBalanceListener() {
        @Override
        public void onBalanceSuccess(String balance) {
            log2Tv("[OnBalanceSuccess] balance=" + balance);
        }

        @Override
        public void onFailure(int statusCode, String errorMessage) {
            log2Tv("[onFailure] statusCode=" + statusCode + ", errorMessage=" + errorMessage);
        }
    };

    IAPurchase.PurchaseListener purchaseListener = new IAPurchase.PurchaseListener() {
        @Override
        public void onPurchaseSuccess(String purchaseId) {
            log2Tv("[onPurchaseSuccess] purchaseId=" + purchaseId);
        }

        @Override
        public void onFailure(int statusCode, String errorMessage) {
            log2Tv("[onFailure] statusCode=" + statusCode + ", errorMessage=" + errorMessage);
        }
    };

    IAPurchase.QueryListener queryListener = new IAPurchase.QueryListener() {
        @Override
        public void onQuerySuccess(IAPurchase.QueryResponse queryResponse) {
            log2Tv("[onQuerySuccess] purchaseId=" + queryResponse.getPurchaseId() + ", status=" + queryResponse.getStatus());
        }

        @Override
        public void onFailure(int statusCode, String errorMessage) {
            log2Tv("[onFailure] statusCode=" + statusCode + ", errorMessage=" + errorMessage);
        }
    };

    IAPurchase.QueryListener2 queryListListener = new IAPurchase.QueryListener2() {
        @Override
        public void onQuerySuccess(IAPurchase.QueryListResponse queryResponse) {
            int total = queryResponse.getTotal();
            List<IAPurchase.QueryResponse2> purchaseList = queryResponse.getPurchaseList();
            log2Tv("[onQueryListSuccess] total = " + total);
            for (IAPurchase.QueryResponse2 qr : purchaseList) {
                log2Tv("purchase_id=" + qr.getPurchaseId() + ", userData=" + qr.getUserData() + ",price=" + qr.getPrice() + ", currency=" + qr.getCurrency() +
                        ", paid_timestamp=" + qr.getPaidTimestamp());
            }
        }

        @Override
        public void onFailure(int statusCode, String errorMessage) {
            log2Tv("[onFailure] statusCode=" + statusCode + ", errorMessage=" + errorMessage);
        }
    };


    @OnClick({R.id.init, R.id.is_ready, R.id.get_balance, R.id.request, R.id.request_userdata, R.id.purchase, R.id.query, R.id.query_purchase_list, R.id.shutdown})
    public void onViewClicked(View view) {
        switch (view.getId()) {
            case R.id.init:
                Api.init(getContext(), initListener, APP_ID);
                break;
            case R.id.is_ready:
                IAPurchase.isReady(getContext(), isReadyListener, APP_KEY);
                break;
            case R.id.get_balance:
                IAPurchase.getBalance(getContext(), getBalanceListener);
                break;
            case R.id.request:
                IAPurchase.request(getContext(), requestListener, "1");
                break;
            case R.id.request_userdata:
                IAPurchase.request(getContext(), requestListener, "1", "Knife");
                break;
            case R.id.purchase:
                IAPurchase.purchase(getContext(), purchaseListener, mPurchaseId);
                break;
            case R.id.query:
                IAPurchase.query(getContext(), queryListener, mPurchaseId);
                break;
            case R.id.query_purchase_list:
                IAPurchase.query(getContext(), queryListListener);
                break;
            case R.id.shutdown:
                Api.shutdown(shutdownListener);
                break;
        }
    }

    private void updateUi() {
        getActivity().runOnUiThread(new Runnable() {
            public void run() {
                init.setEnabled(!mInit);
                isReady.setEnabled(mInit);
                getBalance.setEnabled(mIsReady);
                request.setEnabled(mIsReady);
                requestUserdata.setEnabled(mIsReady);
                purchase.setEnabled(mIsReady);
                query.setEnabled(mIsReady);
                queryPurchaseList.setEnabled(mIsReady);
                shutdown.setEnabled(mInit);
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
        ((MainActivity) getActivity()).onSectionAttached(4);
    }
}
