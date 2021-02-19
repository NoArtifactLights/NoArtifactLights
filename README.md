# NoArtifactLights
From 2/19/2021, this repository is now used instead of another site. Project will be maintained by my main account, [RelaperCrystal](https://github.com/RelaperCrystal), instead of [rcdraft](https://github.com/rcdraft).

![AppVeyor](https://img.shields.io/appveyor/build/rcdraft/noartifactlights?logo=appveyor&style=flat-square)

NoArtifactLights is a GTA5 survival mod based on Blackouts and NPCs fights each other.

## Getting Started

You'll need either a release package or a builded version.

### PlayerCompanion

For built versions, do not copy the DLL file of the PlayerCompanion directly. Download their release instead. The PlayerCompanion is also hosted on GitHub. You can access it from [here](https://github.com/justalemon/PlayerCompanion).

## Contributing

You can either *report issues* or *contribute codes*.
    1. Clone / fork the repository.
    2. Make commit to your fork.
    3. Sent the commits back in pull request.

When contributing code, please do not add new dependencies unless I explicitly authorizes you to do so.

## Building

Clone the repository. Once done, use `devenv NoArtifactLights.sln /Build Release` to build release version which should act like same as release version. ~~From This version onwards, we will implement *feature flags* and turns some feature flags off by default for *Debug* options.~~

You can also use Visual Studio IDE to build. Simply open Visual Studio, clone the repository, and select Debug or Release, then BUILD.
