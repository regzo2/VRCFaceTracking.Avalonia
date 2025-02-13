# VRCFaceTracking.Avalonia

VRCFaceTracking.Avalonia is a cross-platform version of VRCFaceTracking that can be run on Windows, MacOS and Linux. It strives to be as feature complete and similar to the original app as possible.

## Installation

Head to the releases tab and download a release for your operating system.

## Module Compatibility

While VRCFaceTracking.Avalonia can be run on Windows, MacOS and Linux, as it currently stands modules (on the VRCFaceTracking Module Registry) are compiled with the intention of being run on Windows. That being said, at the time of writing this, the following modules in theory *should* be able to be recompiled for MacOS and Linux:

| Module Name                     | Comptatibility | Author(s)               | Version | Last Updated | Module Description                                                                                                      |
|---------------------------------|----------------|-------------------------|---------|--------------|-------------------------------------------------------------------------------------------------------------------------|
| ETVR Tracking Module            | X              | Lorow with ETVR Team    | 0.0.5   | 2024-02-19   | The module translates the eye tracking data from ETVR to VRCFT and vice versa.                                          |
| SteamLink VRCFT Module          | X              | Ykeara                  | 1.0.6   | 2023-12-17   | VRCFT eye and face tracking module for SteamLink on Meta Quest devices.                                                 |
| cympleFaceTracking              | X              | Dominocs                | 1.0     | 2024-03-10   | VRCFaceTracking module for project Cymple.                                                                              |
| ALVR Module                     | X              | zarik5                  | 1.2.0   | 2024-01-26   | VRCFaceTracking module for ALVR support.                                                                                |
| LiveLink                        | X              | Dazbme and tkya         | 1.0.0   | 2023-07-24   | This module allows Perfect Sync (ARKit) data streamed from the Unreal LiveLink iOS app to be used with VRCFaceTracking. |
| ALXR Local Module               | X              | korejan                 | 1.3.2   | 2023-12-14   | Provides eye and/or facial tracking for PC OpenXR runtimes via libalxr.                                                 |
| Meta Quest Link Module          | X              | TofuLemon, azmidi       | 1.4.0   | 2025-02-08   | Provides face tracking for the Quest Pro over Link/AirLink for use with VRCFT.                                          |
| Virtual Desktop                 | X              | Virtual Desktop, Inc.   | 1.3     | 2024-01-30   | Provides face and eye tracking for the Quest Pro when using Virtual Desktop.                                            |
| VRCFT-Babble                    | X              | dfgHiatus               | 2.1.1   | 2023-10-22   | Project Babble face tracking for VRCFaceTracking v5.                                                                    |
| Pico4SAFTExtTrackingModule      | X              | miranda1000, azmidi     | 1.5.1   | 2023-05-05   | Provides eye and face tracking compatibility with the Pico Streaming Assistant.                                         |
| ALXR Remote Module              | X              | korejan                 | 1.3.2   | 2023-12-14   | Provides OpenXR eye and/or facial tracking for standalone & wired headsets via remote ALXR clients.                     |
| SRanipalTrackingModule          | X              | VRCFT Team              | 1.5     | 2024-02-11   | Provides gaze tracking data for SRanipal to interact with VRCFT.                                                        |
| VRCFTPimaxModule                | X              | dfgHiatus               | 1.0     | 2023-04-20   | Droolon Pi 1 eye tracking for VRCFaceTracking v5.                                                                       |
| VRCFTVarjoModule                | X              | Chickenbread & M3gagluk | 4.10    | 2023-04-27   | Provides eye tracking data for Varjo to interact with VRCFT.                                                            |
| ViveStreamingFaceTrackingModule | X              | HTC Corp.               | 1.7     | 2024-11-12   | Streams facial tracking data from Vive Focus 3 and Vive XR Elite to VRCFT's Unified Expressions.                        |
| VRCFTOmniceptModule             | X              | 200Tigersbloxed         | 1.4.0   | 2023-05-23   | Implements support for the HP Reverb G2 Omnicept Eye Tracking with their Glia SDK.                                      |
| MeowFaceTrackingModule          | X              | azmidi                  | 1.3     | 2023-04-20   | Use the MeowFace Android app with VRCFaceTracking!                                                                      |
| iFacialMocap Tracking Module    | X              | Shuisho                 | 1.0     | 2024-03-20   | Provides face tracking data from iFacialMocap for use with VRCFT.                                                       |

Where:

| ✅          | ⚠️            | ❌              |
|------------|---------------|----------------|
| Compatible | Needs Testing | Not Compatible |

## Building

If you want to build from source, clone this repo and open the `.sln`/`.csproj` files in an editor of your choice. I have only tested and built this on Visual Studio 2022 and Rider 2024.3.5, but it should work with other IDEs.

## Credits

- [benaclejames's](https://github.com/benaclejames) [VRCFaceTracking](https://github.com/benaclejames/VRCFaceTracking)
- [MammaMiaDev's](https://github.com/MammaMiaDev) [avaloniaui-the-series](https://github.com/MammaMiaDev/avaloniaui-the-series)
