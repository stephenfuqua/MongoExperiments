var initialData = require("./InitialData.js").InitialData;

var client = require('mongodb').MongoClient;

var url = "mongodb://localhost/IbaMonitoring";

client.connect(url, function (err, db) {
	if (err) { console.log("Error while connecting: " + err); return; }
	
	var surveys = [initialData.FirstSite, initialData.SecondSite];
	
	var collection = db.collection("SurveySite");
	
	ClearAnyExistingRecords(collection, surveys, 
		InsertRecords(collection, surveys, 
			RetrieveTheSavedRecords(collection, 
				CloseTheDatabase(db)
	)))();
	
});

require('paktc'); // PressAnyKeyToContinue

var ClearAnyExistingRecords = function (collection, surveys, callback) {
	return function () {
		collection.deleteMany({}, function (err) {
			if (err) { console.log("ClearAnyExistingRecords: " + err); db.close(); return; }
			
			callback();
		});
	}
}

var InsertRecords = function (collection, surveys, callback) {
	return function () {
		collection.insertMany(surveys, function (err) {
			if (err) { console.log("InsertRecords: " + err); db.close(); return; }
			
			callback();
		});
	}
}

var RetrieveTheSavedRecords = function (collection, callback) {
	return function () {
		collection.find().toArray(function (err, results) {
			if (err) { console.log("RetrieveTheSavedRecords: " + err); db.close(); return; }
			
			WriteOutTheRecords(results);
			
			callback();
		});
	}
}


var WriteOutTheRecords = function (results) {
	
	results.forEach(function (x) {
		console.log(x);
	});
}

var CloseTheDatabase = function (db) {
	return function () {
		console.log("Closing the database connection");
		db.close();
	}
}
