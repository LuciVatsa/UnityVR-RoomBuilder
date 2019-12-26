package com.viveport.myapplication.menu;

import android.content.Context;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseExpandableListAdapter;
import android.widget.TextView;

import java.util.HashMap;
import java.util.List;

public class ExpandableListAdapter extends BaseExpandableListAdapter {

    private Context mContext;
    private List<String> mListDataHeader;
    private HashMap<String, List<String>> mListDataChild;

    public ExpandableListAdapter(Context context, List<String> listDataHeader,
                                 HashMap<String, List<String>> listChildData) {
        this.mContext = context;
        this.mListDataHeader = listDataHeader;
        this.mListDataChild = listChildData;
    }

    @Override
    public int getGroupCount() {
        return mListDataHeader.size();
    }

    @Override
    public int getChildrenCount(int i) {
        if (mListDataChild.get(mListDataHeader.get(i)) == null) return 0;
        return mListDataChild.get(mListDataHeader.get(i)).size();
    }

    @Override
    public Object getGroup(int i) {
        return mListDataHeader.get(i);
    }

    @Override
    public Object getChild(int i, int i1) {
        return mListDataChild.get(mListDataHeader.get(i)).get(i1);
    }

    @Override
    public long getGroupId(int i) {
        return i;
    }

    @Override
    public long getChildId(int i, int i1) {
        return i1;
    }

    @Override
    public boolean hasStableIds() {
        return false;
    }

    @Override
    public View getGroupView(int i, boolean isExpanded, View view, ViewGroup viewGroup) {
        String groupTitle = (String) getGroup(i);
        TextView tv = new TextView(mContext);
        if (getChildrenCount(i) != 0) {
            tv.setText(groupTitle + " >");
        } else {
            tv.setText(groupTitle);
        }
        tv.setTextSize(20);
        tv.setBackgroundColor(0xffffffff);
        tv.setPadding(20,10,10,10);
        return tv;
    }

    @Override
    public View getChildView(int i, int i1, boolean isExpanded, View view, ViewGroup viewGroup) {
        String childText = (String) getChild(i, i1);
        TextView tv = new TextView(mContext);
        tv.setText(childText);
        tv.setTextSize(18);
        tv.setTextColor(0x75000000);
        tv.setBackgroundColor(0xffffffff);
        tv.setPadding(60,10,10,10);
        return tv;
    }

    @Override
    public boolean isChildSelectable(int i, int i1) {
        return true;
    }
}
