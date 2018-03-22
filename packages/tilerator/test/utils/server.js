const BBPromise = require('bluebird');
const ServiceRunner = require('service-runner');
const logStream = require('./logStream');
const fs = require('fs');
const assert = require('./assert');
const yaml = require('js-yaml');
const extend = require('extend');

// set up the configuration
let config = {
  conf: yaml.safeLoad(fs.readFileSync(`${__dirname}/../../config.dev.yaml`)),
};
// build the API endpoint URI by supposing the actual service
// is the last one in the 'services' list in the config file
const myServiceIdx = config.conf.services.length - 1;
const myService = config.conf.services[myServiceIdx];
config.uri = `http://localhost:${myService.conf.port}/`;
config.service = myService;
// no forking, run just one process when testing
config.conf.num_workers = 0;
// have a separate, in-memory logger only
config.conf.logging = {
  name: 'test-log',
  level: 'trace',
  stream: logStream(),
};
// make a deep copy of it for later reference
const origConfig = extend(true, {}, config);

let stop = function stop() {};
let options = null;
const runner = new ServiceRunner();

function start(_options) {
  const normalizedOptions = _options || {};

  if (!assert.isDeepEqual(options, normalizedOptions)) {
    // eslint-disable-next-line no-console
    console.log('server options changed; restarting');
    stop();
    options = normalizedOptions;
    // set up the config
    config = extend(true, {}, origConfig);
    extend(true, config.conf.services[myServiceIdx].conf, options);
    return runner.run(config.conf)
      .then((servers) => {
        const server = servers[0];
        // eslint-disable-next-line no-shadow
        stop = function stop() {
          // eslint-disable-next-line no-console
          console.log('stopping test server');
          server.close();
          // eslint-disable-next-line no-func-assign,no-shadow
          stop = function stop() {};
        };
        return true;
      });
  }
  return BBPromise.resolve();
}

module.exports.config = config;
module.exports.start = start;
