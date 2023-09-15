using Scriptable_objects;
using UI.Scrolling;
using UnityEngine;
using LocationInfo = Scriptable_objects.LocationInfo;

[CreateAssetMenu(fileName = "LevelScrollingPanelFactory", menuName = "Factories/ LevelScrollingPanelFactory")]
public sealed class LevelScrollingPanelFactory : PanelFactory<LocationInfo, LevelPerformer>
{

}