Quantae.ArrowPlotter = {};

/*
Class State
{
	var stateNumber;
	var stateDetail;
	var stateText;
	var arrowNumber;
}
*/

Quantae.ArrowPlotter.States = [];
Quantae.ArrowPlotter.StateStart = 0;
Quantae.ArrowPlotter.StatesRelation = [];
Quantae.ArrowPlotter.ArrowLevelYIndex = 50;
Quantae.ArrowPlotter.GroupLevelYIndex = 15;
Quantae.ArrowPlotter.GroupLevelSpotYIndex = 10;
Quantae.ArrowPlotter.GroupLevelWordYIndex = 5;
Quantae.ArrowPlotter.SingleStates = [];
Quantae.ArrowPlotter.GoupedStates = [];
Quantae.ArrowPlotter.PlottedGrammerEntities = [];
Quantae.ArrowPlotter.PlottedGrammerEntitiesSpot = [];
Quantae.ArrowPlotter.PlottedArrows = [];
Quantae.ArrowPlotter.entriesTextFormat = {
				"font-size" : 30,
				"font-family" : "Verdana",
				"fill" : "red"
			};
Quantae.ArrowPlotter.groupEntriesTextFormat = {
				"font-size" : 20,
				"font-family" : "Verdana",
				"fill" : "black"
			};

Quantae.ArrowPlotter.GrammerEntitySpotMarginTop = 20;
Quantae.ArrowPlotter.GrammerEntitySpotRadii = 4;
Quantae.ArrowPlotter.grammerEntitySpotFormat = {
                "fill" : "#000000",
                "stroke" : "none"
            };

Quantae.ArrowPlotter.TotalLevels = 5;
Quantae.ArrowPlotter.arrowFormat = {
    "fill" : "red"
};

Quantae.ArrowPlotter.EntryType = {};            
Quantae.ArrowPlotter.EntryType.Group = "__GROUP__ENTRY__";
Quantae.ArrowPlotter.EntryType.Single = "__SINGLE__ENTRY__";

Quantae.ArrowPlotter.RadiansToDegree = function(value)
{
    return Number(value * (180/Math.PI));
}

Quantae.ArrowPlotter.DegreeToRadians = function(value)
{
    return Number(Math.PI * (value/180));
}
            
Quantae.ArrowPlotter.GroupEntry = function(text)
{
    this.text = text;
    this.type = Quantae.ArrowPlotter.EntryType.Group;
}

Quantae.ArrowPlotter.SingleEntry = function(text)
{
    this.text = text;
    this.type = Quantae.ArrowPlotter.EntryType.Single;
}

Quantae.ArrowPlotter.GroupEntrySpot = function(spot)
{
    this.spot = spot;
    this.type = Quantae.ArrowPlotter.EntryType.Group;
}

Quantae.ArrowPlotter.SingleEntrySpot = function(spot)
{
    this.spot = spot;
    this.type = Quantae.ArrowPlotter.EntryType.Single;
}

Quantae.ArrowPlotter.GetMaxArrowHeight = function()
{
    var max = -1;
    
    if (typeof this.arrows == 'undefined')
        return max;
    
    for (var i = 0; i < this.arrows.length; i++)
    {
        if (this.arrows[i].arrow.arrowHeight > max)
            max = this.arrows[i].arrow.arrowHeight;
    }
    
    return max;
}

Quantae.ArrowPlotter.AddState = function(state)
{
	Quantae.ArrowPlotter.States.push({stateNumber : Quantae.ArrowPlotter.StateStart, stateDetail : state, getMaxArrowHeight : Quantae.ArrowPlotter.GetMaxArrowHeight});
    
    if (typeof state.length == 'undefined')
        Quantae.ArrowPlotter.SingleStates[state] = Quantae.ArrowPlotter.StateStart;
    else
        Quantae.ArrowPlotter.GoupedStates.push(Quantae.ArrowPlotter.StateStart);
        
	return Quantae.ArrowPlotter.StateStart++;
}

Quantae.ArrowPlotter.FetchState = function(state)
{
	for (var i = 0; i < Quantae.ArrowPlotter.States.length; i++)
	{
		if (Quantae.ArrowPlotter.States[i].stateDetail == state)
		{
			return Quantae.ArrowPlotter.States[i].stateNumber;
		}
	}
	
	return Quantae.ArrowPlotter.AddState(state);
}

Quantae.ArrowPlotter.AssociateTextWithState = function(state, text)
{
	Quantae.ArrowPlotter.States[state].stateText = text;
}

Quantae.ArrowPlotter.Analyze = function(grammerAnalysis, mapElement)
{
	var currStateNumOne = null;
	var currStateNumTwo = null;
	
	for (var i = 0; i < grammerAnalysis.length; i++)
	{
		currStateNumOne = Quantae.ArrowPlotter.FetchState(grammerAnalysis[i].startIndex);
		Quantae.ArrowPlotter.AssociateTextWithState(currStateNumOne, grammerAnalysis[i].roleStart);
		
		currStateNumTwo = Quantae.ArrowPlotter.FetchState(grammerAnalysis[i].endIndex);
		Quantae.ArrowPlotter.AssociateTextWithState(currStateNumTwo, grammerAnalysis[i].roleEnd);
		
		if (typeof Quantae.ArrowPlotter.StatesRelation[currStateNumOne] == 'undefined')
			Quantae.ArrowPlotter.StatesRelation[currStateNumOne] = [];

		Quantae.ArrowPlotter.StatesRelation[currStateNumOne][currStateNumTwo] = true;
	}
    
    var mapString = '<table border="1">';
    mapString += '<tr>'
    mapString += '<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>';
    
    for (var i = 0; i < Quantae.ArrowPlotter.States.length; i++)
	{
	   mapString += '<td>' + Quantae.ArrowPlotter.States[i].stateDetail + '</td>';
    }
    mapString += '</tr>'
	
	for (var i = 0; i < Quantae.ArrowPlotter.States.length; i++)
	{
        mapString += '<tr>'
        mapString += '<td>' + Quantae.ArrowPlotter.States[i].stateDetail + ' [ ' + Quantae.ArrowPlotter.States[i].stateNumber + ' ] </td>';
        
		if (typeof Quantae.ArrowPlotter.StatesRelation[i] == 'undefined')
		{
			Quantae.ArrowPlotter.StatesRelation[i] = [];
			for (var j = 0; j < Quantae.ArrowPlotter.States.length; j++)
			{
				Quantae.ArrowPlotter.StatesRelation[i][j] = false;
                mapString += '<td>0</td>';
			}
		}
		else
		{
			for (var j = 0; j < Quantae.ArrowPlotter.States.length; j++)
			{
				if (Quantae.ArrowPlotter.StatesRelation[i][j] != true)
                {
					Quantae.ArrowPlotter.StatesRelation[i][j] = false;
                    mapString += '<td>0</td>';
                }
                else
                {
                    mapString += '<td>1</td>';
                }
			}
		}
        
        mapString += '</tr>';
	}
    
    mapString += '</table>';
    
    mapElement.innerHTML = mapString;
    
    Quantae.ArrowPlotter.TotalLevels = Quantae.ArrowPlotter.States.length;
}

Quantae.ArrowPlotter.AnalyzePreConditions = function(state)
{
	if (typeof Quantae.ArrowPlotter.States[state].length == 'undefined')
	{
		// single state
	}
	else
	{
		// multiple states
	}
}

Quantae.ArrowPlotter.PlotSpot = function(x, y, radius, paperToDraw, spotProperties)
{
    var savedSpot = {};
    savedSpot.x = x;
    savedSpot.y = y;
    savedSpot.modifiedX = x + radius;
    savedSpot.modifiedY = y + radius;
    savedSpot.circle = paperToDraw.circle(savedSpot.modifiedX, savedSpot.modifiedY, radius).attr(spotProperties);
    savedSpot.radius = radius;
    savedSpot.width = savedSpot.circle.getBBox().width;
	savedSpot.height = savedSpot.circle.getBBox().height;
    
    if (Quantae.DebugMode == 1)
    {
        var rectBox = paperToDraw.rect(x, y, savedSpot.width, savedSpot.height);
    	rectBox.attr("stroke", "#000000");
        
        savedSpot.outerRect = rectBox;
     }
     
     return savedSpot;
}

Quantae.ArrowPlotter.PlotArrows = function(paperToDraw, plottedWords, plottedMeanings, MarginY)
{
    Quantae.ArrowPlotter.PlotSingleStateSpotsAndWords(paperToDraw, plottedWords, plottedMeanings, MarginY);
    Quantae.ArrowPlotter.PlotSingleStateArrows(paperToDraw);
    Quantae.ArrowPlotter.PlotGroupStateSpotsAndWords(paperToDraw, Quantae.ArrowPlotter.GroupLevelYIndex);
    //Quantae.ArrowPlotter.PlotGroupStateArrows(paperToDraw);
    Quantae.ArrowPlotter.PlotGroupToWordArrows(paperToDraw);
    Quantae.ArrowPlotter.PlotGroupToGroupArrows(paperToDraw);
}

Quantae.ArrowPlotter.PlotSingleStateSpotsAndWords = function(paperToDraw, plottedWords, plottedMeanings, MarginY)
{
    var currState = null;
    var entryX = null;
    var entryY = null;
    
    var spotX = null;
    var spotY = null;
    
    var estEntry = null;
    var currEntry = null;
    var currSpot = null;
    
    for (var i = 0; i < Quantae.ArrowPlotter.SingleStates.length; i++)
    {
        currState = Quantae.ArrowPlotter.SingleStates[i];
        if (typeof currState != 'undefined')
        {
            estEntry = Quantae.getTextProperties(Quantae.ArrowPlotter.States[currState].stateText, paperToDraw, Quantae.ArrowPlotter.entriesTextFormat);
            
            entryX = plottedWords[i].textX + (Number(plottedWords[i].textWidth / 2) - Number(estEntry.textWidth / 2));
            entryY = plottedMeanings[i].textY + plottedMeanings[i].textHeight + MarginY;
            currEntry = Quantae.plotText(entryX, entryY, Quantae.ArrowPlotter.States[currState].stateText, paperToDraw, Quantae.ArrowPlotter.entriesTextFormat);
            
            Quantae.ArrowPlotter.States[currState].word = currEntry;
            
            Quantae.ArrowPlotter.PlottedGrammerEntities.push(
                new Quantae.ArrowPlotter.SingleEntry(
                        currEntry
                    )
                );
            
            spotX = currEntry.textX + (Number(currEntry.textWidth / 2) - Number(Quantae.ArrowPlotter.GrammerEntitySpotRadii));
            spotY = currEntry.textHeight + currEntry.textY + Quantae.ArrowPlotter.GrammerEntitySpotMarginTop;
            currSpot = Quantae.ArrowPlotter.PlotSpot(spotX, spotY, Quantae.ArrowPlotter.GrammerEntitySpotRadii, paperToDraw, Quantae.ArrowPlotter.grammerEntitySpotFormat);
            
            Quantae.ArrowPlotter.States[currState].spot = currSpot;
            
            Quantae.ArrowPlotter.PlottedGrammerEntities.push(
                new Quantae.ArrowPlotter.SingleEntrySpot(
                        currSpot
                    )
                );
        }
    }
	// Analyze preconditions
	// Plot arrows (identify the level of the arc)
	// remove paths from the plotted arrow states
}

Quantae.ArrowPlotter.PlotWord2Word = function(paperToDraw, indexFrom, indexTo, spotRadii)
{
    var difference = indexFrom - indexTo;

    var state1 = null;
    var state2 = null;
    
    if (difference < 0)
    {
        state2 = Quantae.ArrowPlotter.FetchState(indexFrom);
        // Quantae.ArrowPlotter.SingleStates[indexFrom];
        state1 = Quantae.ArrowPlotter.FetchState(indexTo);
        // Quantae.ArrowPlotter.SingleStates[indexTo];
        
        difference *= -1; 
    }
    else
    {
        state1 = Quantae.ArrowPlotter.FetchState(indexFrom);
        state2 = Quantae.ArrowPlotter.FetchState(indexTo);
    }
    
    var startPoint = {
        x: Quantae.ArrowPlotter.States[state1].spot.x + spotRadii,
        y: Quantae.ArrowPlotter.States[state1].spot.y + (spotRadii * 2)
    };
    var endPoint = {
        x: Quantae.ArrowPlotter.States[state2].spot.x + spotRadii,
        y: Quantae.ArrowPlotter.States[state2].spot.y + (spotRadii * 2)
    };
    
    if (typeof Quantae.ArrowPlotter.States[state1].arrows == 'undefined')
    {
        Quantae.ArrowPlotter.States[state1].arrows = [];
    }
    
    state1 = Quantae.ArrowPlotter.FetchState(indexFrom);
    state2 = Quantae.ArrowPlotter.States[Quantae.ArrowPlotter.FetchState(indexTo)];
    
    Quantae.ArrowPlotter.States[state1].arrows.push(
    {
        arrow : Quantae.ArrowPlotter.PlotArrow(paperToDraw, startPoint, endPoint, difference),
        arrowHead : Quantae.ArrowPlotter.PlotArrowHead(paperToDraw, {x : (state2.spot.x + spotRadii), y : (state2.spot.y + (spotRadii * 2))})
    });
}

Quantae.ArrowPlotter.PlotGroup2Word = function(paperToDraw, indexFrom, indexTo, spotRadii)
{
    var state1 = Quantae.ArrowPlotter.FetchState(indexFrom);
    var state2 = Quantae.ArrowPlotter.FetchState(indexTo);
    
    var spot1 = Quantae.ArrowPlotter.States[state1].spot;
    var spot2 = Quantae.ArrowPlotter.States[state2].spot;
    
    var rad = Math.atan2(Number(spot2.y - spot1.y), Number(spot2.x - spot1.x));
    var degress = Quantae.ArrowPlotter.RadiansToDegree(rad);
    
    var options = {
        pointOne : { 
            x : Quantae.ArrowPlotter.States[state1].spot.x + spotRadii,
            y: Quantae.ArrowPlotter.States[state1].spot.y + (spotRadii * 2)
        },
        pointTwo : { 
            x : Quantae.ArrowPlotter.States[state2].spot.x + spotRadii,
            y: Quantae.ArrowPlotter.States[state2].spot.y + (spotRadii * 2)
        },
        quadrant : {
            x : 1,
            y : 0
        },
        angle : degress,
        levelRatio : {
            x : 30,
            y : 4
        }
    };
    
    if (typeof Quantae.ArrowPlotter.States[Quantae.ArrowPlotter.FetchState(indexFrom)].arrows == 'undefined')
    {
        Quantae.ArrowPlotter.States[Quantae.ArrowPlotter.FetchState(indexFrom)].arrows = [];
    }
    
    Quantae.ArrowPlotter.States[Quantae.ArrowPlotter.FetchState(indexFrom)].arrows.push(
    {
            arrow : Quantae.ArrowPlotter.PlotArrowWithOptions(paperToDraw, options),
            arrowHead : Quantae.ArrowPlotter.PlotArrowHead(paperToDraw, {x : (spot2.x + spotRadii), y : (spot2.y + (spotRadii * 2))})
    });
    // plotArrow; with level as difference
}

Quantae.ArrowPlotter.PlotGroup2Group = function(paperToDraw, indexFrom, indexTo, spotRadii)
{
    var state1 = Quantae.ArrowPlotter.FetchState(indexFrom);
    var state2 = Quantae.ArrowPlotter.FetchState(indexTo);
    var temp = null;
    
    var state1Arr = Quantae.ArrowPlotter.States[state1].stateDetail.sort();
    var state2Arr = Quantae.ArrowPlotter.States[state2].stateDetail.sort();
    
    if (state1Arr[0] > state2Arr[0])
    {
        temp = state1Arr;
        state1Arr = state2Arr;
        state2Arr = temp;
    }
    
    var level = state2Arr[0] - state1Arr[(state1Arr.length - 1)];
    
    var spot1 = Quantae.ArrowPlotter.States[state1].spot;
    var spot2 = Quantae.ArrowPlotter.States[state2].spot;
    
    var rad = Math.atan2(Number(spot2.y - spot1.y), Number(spot2.x - spot1.x));
    var degress = Quantae.ArrowPlotter.RadiansToDegree(rad);
    
    var options = {
        pointOne : { 
            x : spot1.x + spotRadii,
            y: spot1.y + (spotRadii * 2)
        },
        pointTwo : { 
            x : spot2.x + spotRadii,
            y: spot2.y + (spotRadii * 2)
        },
        quadrant : {
            x : 1,
            y : 0
        },
        angle : degress,
        levelRatio : {
            x : Quantae.ArrowPlotter.TotalLevels,
            y : level
        }
    };
    
    if (typeof Quantae.ArrowPlotter.States[state1].arrows == 'undefined')
    {
        Quantae.ArrowPlotter.States[state1].arrows = [];
    }
    
    Quantae.ArrowPlotter.States[state1].arrows.push(
    {
            arrow : Quantae.ArrowPlotter.PlotArrowWithOptions(paperToDraw, options),
            arrowHead : Quantae.ArrowPlotter.PlotArrowHead(paperToDraw, {x : (spot2.x + spotRadii), y : (spot2.y + (spotRadii * 2))})
    });
}

Quantae.ArrowPlotter.PlotArrowWithOptions = function(paperToDraw, options)
{
    var retObj = {};
    var pathStr = "M" + options.pointOne.x + "," + options.pointOne.y + " " +
                                    "A" + options.levelRatio.x + "," + options.levelRatio.y + " " +
                                    options.angle + " " +
                                    options.quadrant.x + "," + options.quadrant.y + " " +
                                    options.pointTwo.x + "," + options.pointTwo.y;
    var arrow = paperToDraw.path(pathStr);
    retObj.arrow = arrow;
    retObj.txtPath = pathStr;
    retObj.level = options.levelRatio;
    retObj.arrowHeight = arrow.getBBox().height;
    retObj.arrowWidth = arrow.getBBox().width;
    
    if (Quantae.DebugMode == 1)
    {
        var rectWidth = arrow.getBBox().width;
    	var rectHeight = arrow.getBBox().height;
        var rectX = arrow.getBBox().x;
        var rectY = arrow.getBBox().y;
        
        var rectBox = paperToDraw.rect(rectX, rectY, rectWidth, rectHeight);
    	rectBox.attr("stroke", "#000000");
        
        retObj.outerRect = rectBox;
     }
     return retObj;
}

Quantae.ArrowPlotter.PlotArrow = function(paperToDraw, pointOne, pointTwo, level)
{
    var retObj = {};
    var pathStr = "M" + pointOne.x + "," + pointOne.y + " " +
                                    "A" + Quantae.ArrowPlotter.TotalLevels + "," + level + " " +
                                    "0" + " " +
                                    "0, 0" + " " +
                                    pointTwo.x + "," + pointTwo.y;
    var arrow = paperToDraw.path(pathStr);
    retObj.arrow = arrow;
    retObj.txtPath = pathStr;
    retObj.level = level;
    retObj.arrowHeight = arrow.getBBox().height;
    retObj.arrowWidth = arrow.getBBox().width;
    
    if (Quantae.DebugMode == 1)
    {
        var rectWidth = arrow.getBBox().width;
    	var rectHeight = arrow.getBBox().height;
        var rectX = arrow.getBBox().x;
        var rectY = arrow.getBBox().y;
        
        var rectBox = paperToDraw.rect(rectX, rectY, rectWidth, rectHeight);
    	rectBox.attr("stroke", "#000000");
        
        retObj.outerRect = rectBox;
     }
     return retObj;
}

Quantae.ArrowPlotter.PlotLine = function(paperToDraw, pointOne, pointTwo)
{
    var retObj = {};
    var pathStr = "M" + pointOne.x + " " + pointOne.y +
                    "L" + pointTwo.x + " " + pointTwo.y;
    var line = paperToDraw.path(pathStr);
    retObj.line = line;
    retObj.txtPath = pathStr;
    retObj.lineHeight = line.getBBox().height;
    retObj.lineWidth = line.getBBox().width;
    retObj.lineDrawn = {
        p1 : pointOne,
        p2 : pointTwo
    };
    
    if (Quantae.DebugMode == 1)
    {
        var rectWidth = line.getBBox().width;
    	var rectHeight = line.getBBox().height;
        var rectX = line.getBBox().x;
        var rectY = line.getBBox().y;
        
        var rectBox = paperToDraw.rect(rectX, rectY, rectWidth, rectHeight);
    	rectBox.attr("stroke", "#000000");
        
        retObj.outerRect = rectBox;
     }
     return retObj;
}

Quantae.ArrowPlotter.PlotArrowHead = function(paperToDraw, point)
{
    var retObj = {};
    var arrowX = 5;
    var arrowY = 6;

    var pathStr = "M" + point.x + "," + point.y + " " +
                    "m" + (arrowX * -1) + "," + arrowY + " " +
                    "l" + arrowX + "," + (arrowY * -1) + " " +
                    "m" + arrowX + "," + arrowY + " " +
                    "l" + (arrowX * -1) + "," + (arrowY * -1);
    var arrow = paperToDraw.path(pathStr);
    retObj.arrowHead = arrow;
    retObj.txtPath = pathStr;
    retObj.arrowHeight = arrow.getBBox().height;
    retObj.arrowWidth = arrow.getBBox().width;
    retObj.point = point;
    
    if (Quantae.DebugMode == 1)
    {
        var rectWidth = arrow.getBBox().width;
    	var rectHeight = arrow.getBBox().height;
        var rectX = arrow.getBBox().x;
        var rectY = arrow.getBBox().y;
        
        var rectBox = paperToDraw.rect(rectX, rectY, rectWidth, rectHeight);
    	rectBox.attr("stroke", "#000000");
        
        retObj.outerRect = rectBox;
     }
     
     return retObj;
}

Quantae.ArrowPlotter.PlotSingleStateArrows = function(paperToDraw)
{
    var state1 = null;
    var state2 = null;
    
    for (var i = 0; i < Quantae.ArrowPlotter.SingleStates.length; i++)
    {
        state1 = Quantae.ArrowPlotter.FetchState(i);
        for (var j = 0; j < Quantae.ArrowPlotter.SingleStates.length; j++)
        {
            if (i == j)
                continue;

            state2 = Quantae.ArrowPlotter.FetchState(j);
            
            if (Quantae.ArrowPlotter.StatesRelation[state1][state2])
                Quantae.ArrowPlotter.PlotWord2Word(paperToDraw, i, j, Quantae.ArrowPlotter.GrammerEntitySpotRadii);
        }
    }
}

Quantae.ArrowPlotter.PlotGroupStateSpotsAndWords = function(paperToDraw, MarginY)
{
    var currState = null;
    var statesArray = null;
    
    var minPoint = null;
    var maxPoint = null;
    var minSpot = null;
    var maxSpot = null;
    
    var entryX = null;
    var entryY = null;
    
    var spotX = null;
    var spotY = null;
    
    var estEntry = null;
    
    var currEntry = null;
    var currSpot = null;
    
    var tempArrowHeight = null;
    
    for (var i = 0; i < Quantae.ArrowPlotter.GoupedStates.length; i++)
    {
        currState = Quantae.ArrowPlotter.GoupedStates[i];
        statesArray = Quantae.ArrowPlotter.States[currState].stateDetail.sort();

        minSpot = Quantae.ArrowPlotter.States[Quantae.ArrowPlotter.FetchState(statesArray[0])].spot;
        maxSpot = Quantae.ArrowPlotter.States[Quantae.ArrowPlotter.FetchState(statesArray[(statesArray.length - 1)])].spot;
        
        if (minSpot.x > maxSpot.x)
        {
            tempArrowHeight = minSpot;
            minSpot = maxSpot;
            maxSpot = tempArrowHeight;
        }
        
        var maxHeightArrow = -1;
        
        for (var j = 0; j < statesArray.length; j++)
        {
            tempArrowHeight = Quantae.ArrowPlotter.States[Quantae.ArrowPlotter.FetchState(statesArray[j])].getMaxArrowHeight();
            if (tempArrowHeight > maxHeightArrow)
                maxHeightArrow = tempArrowHeight;
        }
        
        var lineDrawn = {
            x1 : Number(minSpot.x + minSpot.radius),
            y1 : Number(minSpot.y + MarginY + maxHeightArrow),
            x2 : Number(maxSpot.x + minSpot.radius),
            y2 : Number(maxSpot.y + MarginY + maxHeightArrow)
        };

        Quantae.ArrowPlotter.States[currState].line = Quantae.ArrowPlotter.PlotLine(paperToDraw, {x : lineDrawn.x1, y : lineDrawn.y1}, {x : lineDrawn.x2, y : lineDrawn.y2});
        
        spotX = lineDrawn.x1 + (Number(Number(lineDrawn.x2 - lineDrawn.x1) / 2) - Number(Quantae.ArrowPlotter.GrammerEntitySpotRadii));
        spotY = lineDrawn.y1 + Quantae.ArrowPlotter.GroupLevelSpotYIndex;
        currSpot = Quantae.ArrowPlotter.PlotSpot(spotX, spotY, Quantae.ArrowPlotter.GrammerEntitySpotRadii, paperToDraw, Quantae.ArrowPlotter.grammerEntitySpotFormat);
        
        Quantae.ArrowPlotter.States[currState].spot = currSpot;
        
        Quantae.ArrowPlotter.PlottedGrammerEntities.push(
            new Quantae.ArrowPlotter.GroupEntrySpot(
                    currSpot
                )
            );

        
        estEntry = Quantae.getTextProperties(Quantae.ArrowPlotter.States[currState].stateText, paperToDraw, Quantae.ArrowPlotter.groupEntriesTextFormat);

        entryX = lineDrawn.x1 + (Number((lineDrawn.x2 - lineDrawn.x1) / 2) - Number(estEntry.textWidth / 2));
        entryY = spotY + currSpot.height + Quantae.ArrowPlotter.GroupLevelWordYIndex;
        currEntry = Quantae.plotText(entryX, entryY, Quantae.ArrowPlotter.States[currState].stateText, paperToDraw, Quantae.ArrowPlotter.groupEntriesTextFormat);
        
        Quantae.ArrowPlotter.States[currState].word = currEntry;
        
        Quantae.ArrowPlotter.PlottedGrammerEntities.push(
            new Quantae.ArrowPlotter.GroupEntry(
                    currEntry
                )
            );
    }
}

Quantae.ArrowPlotter.PlotGroupToWordArrows = function(paperToDraw)
{
    var state1 = null;
    
    for (var i = 0; i < Quantae.ArrowPlotter.GoupedStates.length; i++)
    {
        for (var j = 0; j < Quantae.ArrowPlotter.SingleStates.length; j++)
        {
            state1 = Quantae.ArrowPlotter.FetchState(j);
            
            if (Quantae.ArrowPlotter.StatesRelation[i][state1])
            {
                Quantae.ArrowPlotter.PlotGroup2Word(paperToDraw, Quantae.ArrowPlotter.States[i].stateDetail, j, Quantae.ArrowPlotter.GrammerEntitySpotRadii);
            }
        }
    }
}

Quantae.ArrowPlotter.PlotGroupToGroupArrows = function(paperToDraw)
{    
    for (var i = 0; i < Quantae.ArrowPlotter.GoupedStates.length; i++)
    {
        for (var j = 0; j < Quantae.ArrowPlotter.GoupedStates.length; j++)
        {
            if (i == j)
                continue;
            
            if (Quantae.ArrowPlotter.StatesRelation[i][j])
                Quantae.ArrowPlotter.PlotGroup2Group(paperToDraw, Quantae.ArrowPlotter.States[i].stateDetail, Quantae.ArrowPlotter.States[j].stateDetail, Quantae.ArrowPlotter.GrammerEntitySpotRadii);
        }
    }
}