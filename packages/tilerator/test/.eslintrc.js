module.exports = {
  extends: '../.eslintrc.js',
  rules: {
    // It is okay to import devDependencies in tests.
    'import/no-extraneous-dependencies': [1, { devDependencies: true }],
  },
};
