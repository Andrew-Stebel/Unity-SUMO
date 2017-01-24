Google Maps for Unity
---------------------

Support: forum.differentmethods.com

Note: Google Maps for Unity requires UniWeb: http://u3d.as/1Cw
This is because the Google Maps API requires the use and respect of cache
control headers, which are unsupported by the builtin WWW class.

Because of this, this package will not work in WebPlayer builds as google
does not provide a Cross Domain policy server.

Getting Started
---------------

1. Assign the GoogleMap component to your game object.

2. Setup the parameters in the inspector.
2.1 If you want to control the center point and zoom level, make sure that
    the Auto Locate Center box is unchecked. Otherwise the center point is
    calculated using Markers and Path parameters.

3. Each location field can be an address or longitude / latitude.

4. The markers add pins onto the map, with a single letter label. This label
will only display on mid size markers.

5. The paths add straight lines on the map, between a set of locations.

6. For in depth information on how the GoogleMap component uses the Google
Maps API, see: 
https://developers.google.com/maps/documentation/staticmaps/#quick_example
