# Spherical Galaga

## Intention

For a game jam we wanted to explore how a traditional 2D game would be affected when brought into the 3D dimension.
We decided an interesting way to explore this concept would be to recreate Galaga on a sphere, and see what technical challenges / game mechanics changes would need to be made to make it compelling (while also simply having fun with spherical navigation).

## Implementation

We ran into a few barriers on the journey to get the game up and running, but we found one of the easiest ways to navigate around the sphere was using spherical coordinates with a rotation oriented at one of the poles.
There were a few drawbacks with this implementation as it created the effect of having singularities at the poles. These singularities would have the effect that all of the enemies and character would pass through either pole, and having Gimbal Lock if the loss of a dimension wasn't handled correctly. The spherical coordinate system also lead to a difficulty in describing movement in an arbitrary straight line (which we solved by describing this movement outside of the coordinate system).  

## Continuing Forward

In the end I was impressed with what our team of 3 was able to accomplish in a weekend. If you want to know more, or want to implement something similar and want to know anything we learned during the process let me know. I'll help you out as much as I can.

## Download

I will upload a release for mac, windows, and linux that you should be able to download if you want to try out the game.
