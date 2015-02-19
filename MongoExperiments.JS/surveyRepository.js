﻿SurveyRepository = function (db) {
	db = db;
	collection = db.collection("SurveySite");

	this.deleteAllRecords = function (callback) {
		return function () {
			console.log("Starting to delete");

			this.collection.deleteMany({}, function (err) {
				if (err) { console.log("deleteAllRecords: " + err); this.db.close(); return; }

				callback();
			});
		}
	}

	this.insert = function (surveys, callback) {
		return function () {
			if (surveys.constructor === Array) {
				this.collection.insertMany(surveys, function (err) {
					if (err) { console.log("InsertRecords: " + err); this.db.close(); return; }

					callback();
				});
			}
			else {
				this.collection.insert(surveys, function (err) {
					if (err) { console.log("insert: " + err); this.db.close(); return; }

					callback();
				});
			}
		}
	}


	this.getAll = function (callback) {
		return function () {
			this.collection.find().toArray(function (err, results) {
				if (err) { console.log("RetrieveTheSavedRecords: " + err); this.db.close(); return; }

				callback(results);
			});
		}
	}

	this.toString = function () {
		return "I am a repository for accessing the SurveySite table";
	}
}

module.exports = SurveyRepository;