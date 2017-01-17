var
  where = 'client' // Adds files only to the client
;

Package.describe({
  name    : 'semantic:ui',
  summary : 'Semantic Helpers - LESS Release of Semantic Helpers',
  version : '{version}',
  git     : 'git://github.com/Semantic-Org/Semantic-Helpers-LESS.git',
});

Package.onUse(function(api) {

  api.versionsFrom('1.0');
  api.use('less', 'client');

  api.addFiles([
    {files}
  ], 'client');

});
