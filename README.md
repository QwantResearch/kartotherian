# Kartotherian 

Kartotherian is a map tile service originally built for the [Wikimedia](https://www.wikimedia.org/) projects. Its primary components include:
* Kartotherian ([readme](packages/kartotherian/README.md)): a map tile server tying together various open source modules from the [TileLive](https://github.com/mapbox/tilelive) ecosystem, thereby providing for serving tiles from a variety of sources.
* Tilerator ([readme](packages/tilerator/README.md)): a job scheduler used to schedule asynchronous map tile generation jobs, offering both a command-line and a GUI interface.

This is a [monorepo](https://en.wikipedia.org/wiki/Monorepo) containing (in the `packages/` subdirectory) the various modules developed as part of the Kartotherian project and used in Kartotherian and Tilerator. The repo is managed using [Lerna](https://github.com/lerna/lerna).
Dependencies between packages are managed by using `file:` specifiers in "package.json" files.
To install the dependencies for all modules, run `npm install` from the project root.

The Kartotherian and Tilerator services can then be started accoring to the instructions provided in their individual READMEs.

**IMPORTANT:** Installing and running the Kartotherian services requires Node 8. Node.js 10+ is not yet supported.
