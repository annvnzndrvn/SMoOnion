# SMoOnion
Stop-motion animation software for DSLR cameras. This project was born out of personal need for a stop-motion software with onion-skin features for high quality DSLR cameras. Most stop-motion tools are highly priced and out of reach for students or stop-motion hobbyists. This project aims to address that issue.

This project uses [Nikon SDK C# Wrapper](https://sourceforge.net/p/nikoncswrapper/wiki/Home/) by Thomas Dideriksen.

### Current features:
+ Single frame snapshot
+ LiveView support
+ Onion skin for animation
+ Battery level indicator

### Future features:
+ Onion skin visible up to three previous frames
+ Import video for rotoscoping
+ Support for rest of cameras (currently only Nikon D90 supported)

### Known issues and improvements:
+ Onion skin on last frame not getting correct opacity level (fixed)
+ Improve timer logic
+ Every time user takes a snapshot, next snap takes slightly longer to process (investigating)

Comments/suggestions: anne.van.zondervan[at]protonmail.com
