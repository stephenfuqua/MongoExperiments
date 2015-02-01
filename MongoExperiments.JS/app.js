var client = require('mongodb').MongoClient;

var url = "mongodb://localhost/IbaMonitoring";

client.connect(url, function (err, db) {
    if (err) { console.log("Error while connecting: " + err); return; }
    
    var surveySite = InitializeFirstSite();
    var surveySite2 = InitializeSecondSite();
    var surveys = [surveySite, surveySite2];
    
    var collection = db.collection("SurveySite");
    
    //ClearAnyExistingRecords
    console.log("ClearAnyExistingRecords");
    collection.deleteMany({}, function (err) {
        if (err) { console.log("ClearAnyExistingRecords: " + err); db.close(); return; }
        
        //InsertRecords
        console.log("InsertRecords");
        collection.insertMany(surveys, function (err) {
            if (err) { console.log("InsertRecords: " + err); db.close(); return; }
            
            //RetrieveTheSavedRecords
            console.log("RetrieveTheSavedRecords");

            collection.find().toArray(function (err, results) {
                if (err) { console.log("RetrieveTheSavedRecords: " + err); db.close(); return; }
                
                console.log("result count: " + results.length);
                
                results.forEach(function (x) {
                    console.log(x);
                });
                
                console.log("Closing the database connection");
                db.close();
            })
            
        });
    });
});

require('paktc'); // PressAnyKeyToContinue



var InitializeFirstSite = function () {
    return {
        "SiteName" : "A Park Somewhere",
        "CodeName" : "abc",
        "SurveyPoints" : [
            { "CodeName" : "P1", "Latitude" : "1.11", "Longitude" : "2.11" },
            { "CodeName" : "P2", "Latitude" : "1.12", "Longitude" : "2.12" },
            { "CodeName" : "P3", "Latitude" : "1.13", "Longitude" : "2.13" },
            { "CodeName" : "P4", "Latitude" : "1.14", "Longitude" : "2.14" },
            { "CodeName" : "P5", "Latitude" : "1.15", "Longitude" : "2.15" },
            { "CodeName" : "P1", "Latitude" : "2.11", "Longitude" : "3.11" }, 
            { "CodeName" : "P2", "Latitude" : "2.12", "Longitude" : "3.12" }, 
            { "CodeName" : "P3", "Latitude" : "2.13", "Longitude" : "3.13" }, 
            { "CodeName" : "P4", "Latitude" : "2.14", "Longitude" : "3.14" }
        ],
        "SiteBoundaries" : [
            { "VertexSequence" : 1, "Latitude" : "1.1", "Longitude" : "2.1" }, 
            { "VertexSequence" : 2, "Latitude" : "1.2", "Longitude" : "2.2" }, 
            { "VertexSequence" : 3, "Latitude" : "1.3", "Longitude" : "2.3" }, 
            { "VertexSequence" : 4, "Latitude" : "1.4", "Longitude" : "2.4" }, 
            { "VertexSequence" : 5, "Latitude" : "1.5", "Longitude" : "2.5" }, 
            { "VertexSequence" : 1, "Latitude" : "2.1", "Longitude" : "3.1" }, 
            { "VertexSequence" : 2, "Latitude" : "2.2", "Longitude" : "3.2" }, 
            { "VertexSequence" : 3, "Latitude" : "2.3", "Longitude" : "3.3" }, 
            { "VertexSequence" : 4, "Latitude" : "2.4", "Longitude" : "3.4" }
        ]
    };
}

var InitializeSecondSite = function () {
    return {
        "SiteName" : "A Different Location",
        "CodeName" : "abd",
        "SurveyPoints" : [
            { "CodeName" : "P_2_1", "Latitude" : "2.11", "Longitude" : "3.11" }, 
            { "CodeName" : "P_2_2", "Latitude" : "2.12", "Longitude" : "3.12" }, 
            { "CodeName" : "P_2_3", "Latitude" : "2.13", "Longitude" : "3.13" }, 
            { "CodeName" : "P_2_4", "Latitude" : "2.14", "Longitude" : "3.14" }
        ],
        "SiteBoundaries" : [
            { "VertexSequence" : 1, "Latitude" : "2.1", "Longitude" : "3.1" }, 
            { "VertexSequence" : 2, "Latitude" : "2.2", "Longitude" : "3.2" }, 
            { "VertexSequence" : 3, "Latitude" : "2.3", "Longitude" : "3.3" }, 
            { "VertexSequence" : 4, "Latitude" : "2.4", "Longitude" : "3.4" }
        ]
    };
}