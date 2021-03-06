#Tilerator service with OSM Postgis source
gen:
  uri: http://eole.geostorm.eu:16534/gen/{z}/{x}/{y}.pbf

# Tiles storage
v2:
  public: true
  formats: [pbf]
  uri: cassandra://
  params:
    maxzoom: 16 # Exclusive: stores up to 15 only
    keyspace: v2
    cp: {var: cassandra-servers}
    username: {var: cassandra-user}
    password: {var: cassandra-pswd}
    repfactor: 4
    durablewrite: 0
    createIfMissing: true

# Direct view of tile storage content
v2view:
  public: true
  formats: [png,json,headers,svg,jpeg]
  scales: [1.3, 1.5, 2, 2.6, 3]
  static: true
  maxheight: 2048
  maxwidth: 2048
  uri: vector://
  xml:
    npmpath: ["@mapbox/mapbox-studio-osm-bright", "project.xml"]
  xmlSetParams:
    source: {ref: v2}

dyn:
  public: true
  formats: [pbf]
  uri: autogen://
  params:
    storage: {ref: v2}
    generator: {ref: gen}
    # Optional:
    mingen: 10  # Only generate tiles if missing within this zoom range
    maxgen: 15
    minstore: 10  # if generated, only store them if within this zoom range 
    maxstore: 15

oz:
  public: true
  formats: [pbf]
  uri: overzoom://
  params:
    source: {ref: dyn}

# Final source
osm-intl:  # This name is the default Kartotherian leaflet test app layer name
  public: true
  formats: [png,json,headers,svg,jpeg]
  scales: [1.3, 1.5, 2, 2.6, 3]
  static: true
  maxheight: 2048
  maxwidth: 2048
  uri: vector://
  xml:
    npmpath: ["@mapbox/mapbox-studio-osm-bright", "project.xml"]
  xmlSetParams:
    source: {ref: oz}

mix:
  uri: demultiplexer://
  public: true
  formats: [pbf]
  params:
    source1: {ref: v2}
    from1: 0
    before1: 9
    source2: {ref: gen}
    from2: 9
    before2: 19

mixview:
  public: true
  formats: [png,json,headers,svg,jpeg]
  scales: [1.3, 1.5, 2, 2.6, 3]
  static: true
  maxheight: 2048
  maxwidth: 2048
  uri: vector://
  xml:
    loader: "@kartotherian/osm-bright-style"
  xmlSetParams:
    source: {ref: mix}

