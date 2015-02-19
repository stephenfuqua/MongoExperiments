var _super = require('./repositoryBase.js').prototype;

SurveyRepository = function (db) {
	_super.constructor.apply(this, [db, "SurveySite"]);
}

module.exports = SurveyRepository;