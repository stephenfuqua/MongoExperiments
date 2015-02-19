var initialData = require("./InitialData.js").InitialData;
var SurveyRepository = require("./surveyRepository.js");
var client = require('mongodb').MongoClient;

var url = "mongodb://localhost/IbaMonitoring";

client.connect(url, function (err, db) {
	if (err) { console.log("Error while connecting: " + err); return; }
	
	var surveys = [initialData.FirstSite, initialData.SecondSite];
	
	var repository = new SurveyRepository(db);
	console.log(repository.toString());
	
	repository.deleteAllRecords(
		repository.insert(surveys,
			repository.getAll(WriteOutTheRecords(
				CloseTheDatabase(db)
			))))();
});

require('paktc'); // PressAnyKeyToContinue

var WriteOutTheRecords = function (callback) {
	return function (results) {
		results.forEach(function (x) {
			console.log(x);
		});
		callback();
	}
}

var CloseTheDatabase = function (db) {
	return function () {
		console.log("Closing the database connection");
		db.close();
	}
}
