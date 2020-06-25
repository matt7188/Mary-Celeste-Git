using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command
{
    public enum DestinationType {Room,Items,Person}
    public DestinationType CommandType;
    public RoomName GoingTo;
    public Items ItemPickupOrDrop;
    public RoomName DropingItem;
    public People Persuing;
}
