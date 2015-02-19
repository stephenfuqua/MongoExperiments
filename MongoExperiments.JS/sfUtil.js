// There are other util packages that provide similar functionality,
// but sometimes you just need to practice...

sf = {
	string: {
		Format: function (pattern, values) {
			var output = pattern;
			
			// there is probably a nicer regular expression way of doing this
			for (var i = 0; i < values.length; i++) {
				output = output.replace("{" + i.toString() + "}", values[i]);
			}
			
			return output;
		}
	}
};

module.exports = sf;