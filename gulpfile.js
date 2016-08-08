
var gulp = require("gulp"),
	$ = require("gulp-load-plugins")({ pattern: "*", lazy: false }),
	publishFolder = 'obj/Publish',
    deployFolder = 'obj/Deploy',
	deployFiles = [
					publishFolder + '/Content/estilos.min.*.css',
					publishFolder + '/Content/DataTables/css/datatables.min.*.css',
					publishFolder + '/Content/Site.min.*.css',
					publishFolder + '/Scripts/scripts.min.*.js',
					publishFolder + '/Scripts/mainapp.min.*.js',
					publishFolder + '/bin/*',
					publishFolder + '/Content/DataTables/images/*.*',
					publishFolder + '/Content/DataTables/swf/*.*',
					publishFolder + '/Content/img/*.*',
					publishFolder + '/app/**/*.html',
					publishFolder + '/fonts/*',
                    publishFolder + '/Emails/*.html',
					publishFolder + '/PdfTemplates/*.pdf',
					publishFolder + '/*.html',
					publishFolder + '/favicon.ico',
					publishFolder + '/permissions.json',
					publishFolder + '/connectionstrings.release.config',
					publishFolder + '/appsettings.release.config',
					publishFolder + '/web.config'
				];

//Limpia carpeta de publicación (interna y local)
gulp.task('clean:publish', function() {
	return $.del(publishFolder + '/**/*');
});

//Publica usando configuración de Release a la carpeta indicada (incluye web.config transforms)
gulp.task('publish', ['clean:publish'], function() {
	return gulp.src('../../*.sln')
		.pipe($.msbuild({
			configuration: 'Release',
			targets: ['Clean', 'Build'],
			toolsVersion: 14.0,
			stdout: true,
			properties: {
				DeployOnBuild: true,
				DeployTarget: "PipelinePreDeployCopyAllFilesToOneFolder",
				_PackageTempDir: publishFolder
			}
		}));

});

//Limpia bundles preexistentes
gulp.task('clean:bundles', function () {
	return $.del([
		publishFolder + '/Content/estilos-*.min.css',
		publishFolder + '/Content/DataTables/css/datatables-*.min.css',
		publishFolder + '/Content/Site-*.min.css',
		publishFolder + '/Scripts/scripts-*.min.js',
		publishFolder + '/Scripts/mainapp-*.min.js'
	]);
});

//Concatena y reemplaza scripts y styles en HTML, minifica y genera cache-buster en HTML y archivos
gulp.task('minifybundle', ['clean:bundles', 'publish'], function () {
	var revAll = new $.revAll({ dontRenameFile: ['.html'] });
	return gulp.src('index.html')
		.pipe($.useref())					//bundle js&css / replace html
		.pipe($.if("*.js", $.uglify()))		//minify js
		.pipe($.if("*.css", $.uglifycss()))	//minify css
		.pipe(revAll.revision())			//cache bust js&css / replace html
		.pipe(gulp.dest(publishFolder));
});

//Limpia carpeta de deploy (externa o de servidor)
gulp.task('clean:deploy', function() {
	return $.del(deployFolder + '/**/*');
});

//Copia archivos de carpeta publicación a deploy
gulp.task('deploy', ['clean:deploy', 'minifybundle'], function() {
	return gulp.src(deployFiles, { base: publishFolder })
		.pipe(gulp.dest(deployFolder));
});