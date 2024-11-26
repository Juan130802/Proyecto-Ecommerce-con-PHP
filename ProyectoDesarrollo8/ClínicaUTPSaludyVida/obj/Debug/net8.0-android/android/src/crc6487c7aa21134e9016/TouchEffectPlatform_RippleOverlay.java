package crc6487c7aa21134e9016;


public class TouchEffectPlatform_RippleOverlay
	extends android.view.View
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_dispatchTouchEvent:(Landroid/view/MotionEvent;)Z:GetDispatchTouchEvent_Landroid_view_MotionEvent_Handler\n" +
			"n_onTouchEvent:(Landroid/view/MotionEvent;)Z:GetOnTouchEvent_Landroid_view_MotionEvent_Handler\n" +
			"";
		mono.android.Runtime.register ("Sharpnado.Tabs.Effects.Droid.TouchEffectPlatform+RippleOverlay, Maui.Tabs", TouchEffectPlatform_RippleOverlay.class, __md_methods);
	}


	public TouchEffectPlatform_RippleOverlay (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == TouchEffectPlatform_RippleOverlay.class) {
			mono.android.TypeManager.Activate ("Sharpnado.Tabs.Effects.Droid.TouchEffectPlatform+RippleOverlay, Maui.Tabs", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, System.Private.CoreLib:System.Int32, System.Private.CoreLib", this, new java.lang.Object[] { p0, p1, p2, p3 });
		}
	}


	public TouchEffectPlatform_RippleOverlay (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == TouchEffectPlatform_RippleOverlay.class) {
			mono.android.TypeManager.Activate ("Sharpnado.Tabs.Effects.Droid.TouchEffectPlatform+RippleOverlay, Maui.Tabs", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, System.Private.CoreLib", this, new java.lang.Object[] { p0, p1, p2 });
		}
	}


	public TouchEffectPlatform_RippleOverlay (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == TouchEffectPlatform_RippleOverlay.class) {
			mono.android.TypeManager.Activate ("Sharpnado.Tabs.Effects.Droid.TouchEffectPlatform+RippleOverlay, Maui.Tabs", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
		}
	}


	public TouchEffectPlatform_RippleOverlay (android.content.Context p0)
	{
		super (p0);
		if (getClass () == TouchEffectPlatform_RippleOverlay.class) {
			mono.android.TypeManager.Activate ("Sharpnado.Tabs.Effects.Droid.TouchEffectPlatform+RippleOverlay, Maui.Tabs", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
		}
	}


	public boolean dispatchTouchEvent (android.view.MotionEvent p0)
	{
		return n_dispatchTouchEvent (p0);
	}

	private native boolean n_dispatchTouchEvent (android.view.MotionEvent p0);


	public boolean onTouchEvent (android.view.MotionEvent p0)
	{
		return n_onTouchEvent (p0);
	}

	private native boolean n_onTouchEvent (android.view.MotionEvent p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
