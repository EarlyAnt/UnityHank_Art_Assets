using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Notification
{
	public Component sender;
	public String name;
	public object data;
    public Notification(object aData) {data = aData; }
	public Notification(Component aSender, String aName) { sender = aSender; name = aName; data = null; }
	public Notification(Component aSender, String aName, object aData) { sender = aSender; name = aName; data = aData; }
}