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
 * Use the {@link IAPSubscription#newInstance} factory method to
 * create an instance of this fragment.
 */
public class IAPSubscription extends Fragment {

    private static final String TAG = IAPSubscription.class.getSimpleName();
    private static final String APP_ID = "d4cea7c0-6afa-438c-a740-49d2287a2b68";
    private static final String APP_KEY = "VDHsbSITQ4-glRl-iQibwOZWGGXp57KI";

    Unbinder unbinder;
    @BindView(R.id.init)
    Button init;
    @BindView(R.id.is_ready)
    Button isReady;
    @BindView(R.id.get_balance)
    Button getBalance;
    @BindView(R.id.request_subscripton)
    Button requestSubscripton;
    @BindView(R.id.request_subscription_with_planid)
    Button requestSubscriptionWithPlanid;
    @BindView(R.id.subscribe)
    Button subscription;
    @BindView(R.id.query_subscription)
    Button querySubscription;
    @BindView(R.id.cancel_subscription)
    Button cancelSubscription;
    @BindView(R.id.shutdown)
    Button shutdown;
    @BindView(R.id.log)
    TextView log;
    @BindView(R.id.scroll_view)
    ScrollView scrollView;


    private boolean mInit = false;
    private boolean mIsReady = false;
    private boolean bIsDuplicatedSubscription = false;
    private String mSubscriptionId = "";

    public IAPSubscription() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @return A new instance of fragment IAP.
     */
    // TODO: Rename and change types and number of parameters
    public static IAPSubscription newInstance() {
        IAPSubscription fragment = new IAPSubscription();
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
        View view = inflater.inflate(R.layout.fragment_iap_1, container, false);
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

    IAPurchase.RequestSubscriptionListener requestSubscriptionListener = new IAPurchase.RequestSubscriptionListener() {
        @Override
        public void onRequestSubscriptionSuccess(String subscriptionId) {
            mSubscriptionId = subscriptionId;
            log2Tv("[OnRequestSubscriptionSuccess] SubscriptionId=" + mSubscriptionId);
        }

        @Override
        public void onFailure(int statusCode, String errorMessage) {
            log2Tv("[onFailure] statusCode=" + statusCode + ", errorMessage=" + errorMessage);
        }
    };

    IAPurchase.RequestSubscriptionWithPlanIDListener requestSubscriptionWithPlanIDListener = new IAPurchase.RequestSubscriptionWithPlanIDListener() {
        @Override
        public void onRequestSubscriptionWithPlanIDSuccess(String subscriptionId) {
            mSubscriptionId = subscriptionId;
            log2Tv("[onRequestSubscriptionWithPlanIDSuccess] SubscriptionId=" + mSubscriptionId);
        }

        @Override
        public void onFailure(int statusCode, String errorMessage) {
            log2Tv("[onFailure] statusCode=" + statusCode + ", errorMessage=" + errorMessage);
        }
    };

    IAPurchase.SubscribeListener subscribeListener = new IAPurchase.SubscribeListener() {
        @Override
        public void onSubscribeSuccess(String subscriptionId) {
            log2Tv("[onSubscribeSuccess] subscriptionId=" + subscriptionId);
        }

        @Override
        public void onFailure(int statusCode, String errorMessage) {
            log2Tv("[onFailure] statusCode=" + statusCode + ", errorMessage=" + errorMessage);
        }
    };

    IAPurchase.QuerySubscriptionListener querySubscriptionListener = new IAPurchase.QuerySubscriptionListener() {

        @Override
        public void onQuerySubscriptionSuccess(List<IAPurchase.Subscription> subscriptions) {
            int size = subscriptions.size();
            log2Tv("[OnQuerySubscriptionSuccess] subscriptionlist size =" + size);
            if (size > 0)
            {
                for (int i = 0; i < size; i++)
                {
                    //when status equals "ACTIVE", then this subscription is valid, you can deliver virtual items to user
                    log2Tv("[OnQuerySubscriptionSuccess] subscriptionlist[" + i + "].status =" + subscriptions.get(i).getStatus() +
                            ", subscriptionlist[" + i + "].plan_id = " + subscriptions.get(i).getPlanId());
                    if (subscriptions.get(i).getPlanId() == "pID" && subscriptions.get(i).getStatus() == "ACTIVE")
                    {
                        bIsDuplicatedSubscription = true;
                    }
                }
            }

        }

        @Override
        public void onFailure(int statusCode, String errorMessage) {
            log2Tv("[onFailure] statusCode=" + statusCode + ", errorMessage=" + errorMessage);
        }
    };

    IAPurchase.CancelSubscriptionListener cancelSubscriptionListener = new IAPurchase.CancelSubscriptionListener() {
        @Override
        public void onCancelSubscriptionSuccess(boolean bCanceled) {
            log2Tv("[OnCancelSubscriptionSuccess] bCanceled=" + bCanceled);
        }

        @Override
        public void onFailure(int statusCode, String errorMessage) {
            log2Tv("[onFailure] statusCode=" + statusCode + ", errorMessage=" + errorMessage);
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


    @OnClick({R.id.init, R.id.is_ready, R.id.get_balance, R.id.request_subscripton, R.id.request_subscription_with_planid, R.id.subscribe, R.id.query_subscription, R.id.cancel_subscription, R.id.shutdown})
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
            case R.id.request_subscripton:
                IAPurchase.requestSubscription(getContext(), requestSubscriptionListener, "1", "month", 1, "day", 2, 3, "pID");
                break;
            case R.id.request_subscription_with_planid:
                IAPurchase.requestSubscriptionWithPlanID(getContext(), requestSubscriptionWithPlanIDListener, "pID");
                break;
            case R.id.subscribe:
                IAPurchase.subscribe(getContext(), subscribeListener, mSubscriptionId);
                break;
            case R.id.query_subscription:
                IAPurchase.querySubscription(getContext(), querySubscriptionListener, mSubscriptionId);
                break;
            case R.id.cancel_subscription:
                IAPurchase.cancelSubscription(getContext(), cancelSubscriptionListener, mSubscriptionId);
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
                requestSubscripton.setEnabled(mIsReady);
                requestSubscriptionWithPlanid.setEnabled(mIsReady);
                subscription.setEnabled(mIsReady);
                querySubscription.setEnabled(mIsReady);
                cancelSubscription.setEnabled(mIsReady);
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
