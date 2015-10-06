//inspired by http://jsfiddle.net/chrisJamesC/HgHqy/

var edges = [];
$.ajax({
	type: "POST",
	dataType: "json",
	traditional: true,
	async: false,
	url: "/graph/edges",
	success: function(results) {
		edges = results;
	}
});

var nodes = {};

// Compute the distinct nodes from the edges.
edges.forEach(function (x) {
	x.source = nodes[x.Source] || (nodes[x.Source] = { id: x.SourceId, name: x.Source });
	x.target = nodes[x.Target] || (nodes[x.Target] = { id: x.TargetId, name: x.Target });
});

var w = 960, h = 500;

var force = d3.layout.force()
    .nodes(d3.values(nodes))
    .links(edges)
    .size([w, h])
    .linkDistance(150)
    .charge(-500)
    .on("tick", drawGraph)
    .start();

var svg = d3.select("div#graph").append("svg:svg").attr("width", w).attr("height", h);

var path = svg.append("svg:g").selectAll("path").data(force.links()).enter()
			  .append("svg:path").attr("class", function () { return "edge"; });

var selected = 0;
var toggleCircle = (function () {
	return function () {
		var d3nodes = d3.values(nodes);
		
		var traversedNodes = d3nodes.filter(function (x) { return x.traversed; });
		if (traversedNodes.length > 0) {
			d3nodes.forEach(function (n) {
				if (!n.selected && !n.traversed) {
					return;
				}
				n.selected = false;
				n.traversed = false;
				var nCircle = circle[0].filter(function (x) { return x.__data__.id == n.id; })[0];
				d3.select(nCircle).style("fill", "white");
			});
			return;
		}
		var selectedNodes = d3nodes.filter(function (x) { return x.selected; });
		
		if (selectedNodes.length <= 2) {
			var node = this.__data__;
			
			node.selected = !node.selected;
			d3.select(this).style("fill", node.selected ? "magenta" : "white");
			if (node.selected) {
				selectedNodes.push(node);
			} else {
				selectedNodes.pop(node);
			}
			
			if (selectedNodes.length == 2) {
				$.ajax({
					type: "POST",
					dataType: "json",
					traditional: true,
					url: "/graph/path",
					data: { from: selectedNodes[0].id, to: selectedNodes[1].id },
					success: function (results) {
						var nodes = JSON.parse(results);
						nodes.forEach(function (id) {
							var pathCircle = circle[0].filter(function (x) { return x.__data__.id == id; })[0];
							var pathNode = pathCircle.__data__;
							
							if (!pathNode.selected) {
								pathNode.traversed = true;
								d3.select(pathCircle).style("fill", "cyan");
							}
						});
					},
					error: function (jqXHR, textStatus, errorThrown) {
						console.error(textStatus + ", check jqXHR for more details");
					}
				});
			}
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
