//inspired by http://jsfiddle.net/chrisJamesC/HgHqy/

var links = [];
$.ajax({
	type: "POST",
	dataType: "json",
	contentType: "application/json; charset=utf-8",
	traditional: true,
	async: false,
	url: "/home/getnodes",
	success: function(results) {
		links = $.parseJSON(results);
	}
});

var nodes = {};

// Compute the distinct nodes from the links.
links.forEach(function (link) {
	link.source = nodes[link.Source] || (nodes[link.Source] = { id: link.SourceId, name: link.Source });
	link.target = nodes[link.Target] || (nodes[link.Target] = { id: link.TargetId, name: link.Target });
});

var w = 960, h = 500;

var force = d3.layout.force()
    .nodes(d3.values(nodes))
    .links(links)
    .size([w, h])
    .linkDistance(150)
    .charge(-500)
    .on("tick", drawGraph)
    .start();

var svg = d3.select("div#graph").append("svg:svg").attr("width", w).attr("height", h);

var path = svg.append("svg:g").selectAll("path").data(force.links()).enter()
			  .append("svg:path").attr("class", function () { return "link resolved"; });

var selected = 0;
var toggleCircle = (function () {
	var currentColor = "white";

	return function () {
		selected += currentColor == "white" ? 1 : -1;

		if (selected < 2) {
			currentColor = currentColor == "white" ? "magenta" : "white";
			d3.select(this).style("fill", currentColor);

			console.log("Clicked on " + this.__data__.name);
		}
	};
})();

var circle = svg.append("svg:g").selectAll("circle").data(force.nodes()).enter()
				.append("svg:circle").attr("r", 20).call(force.drag).style("stroke", "gray").style("fill", "white")
				.on("click", toggleCircle);

var text = svg.append("svg:g").selectAll("g").data(force.nodes()).enter()
			  .append("svg:g");

// A copy of the text with a thick white stroke for legibility.
text.append("svg:text").attr("x", 8).attr("y", ".31em").attr("class", "shadow").text(function (d) { return d.name + "(" + d.id + ")"; });
text.append("svg:text").attr("x", 8).attr("y", ".31em").text(function (d) { return d.name + "(" + d.id + ")"; });

// Use elliptical arc path segments to doubly-encode directionality.
function drawGraph() {
	path.attr("d", function (d) {
		var dx = d.target.x - d.source.x,
			dy = d.target.y - d.source.y,
			dr = Math.sqrt(dx * dx + dy * dy);
		return "M" + d.source.x + "," + d.source.y + "A" + dr + "," + dr + " 0 0,1 " + d.target.x + "," + d.target.y;
	});

	circle.attr("transform", function (d) {
		return "translate(" + d.x + "," + d.y + ")";
	});

	text.attr("transform", function (d) {
		return "translate(" + d.x + "," + d.y + ")";
	});
}
