package md523bf1a004f5e5a12915eeb27ee1a4dc0;


public class Activity1
	extends md59e336b20c5f59a4196ec0611a339f132.AndroidGameActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("Wots.Activity1, Wots, Version=1.0.6655.22372, Culture=neutral, PublicKeyToken=null", Activity1.class, __md_methods);
	}


	public Activity1 ()
	{
		super ();
		if (getClass () == Activity1.class)
			mono.android.TypeManager.Activate ("Wots.Activity1, Wots, Version=1.0.6655.22372, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
