var Quantae = {};
Quantae.DebugMode = 0;

Quantae.getVisualObjectProperties = function(object)
{
    var retObj = {
        x : object.getBBox().x,
        y : object.getBBox().y,
        width : object.getBBox().width,
        height : object.getBBox().height
    };
    
    return retObj;
}

Quantae.getTextProperties = function(text, paperToDraw, textProperties)
{
	var retObj = {};
	var textX = 0;
	var textY = 0;
	
	var textDrawn = paperToDraw.text(textX, textY, text);

	textProperties["text-anchor"] = "start";
	textDrawn.attr(textProperties);

	retObj.textWidth = textDrawn.getBBox().width;
	retObj.textHeight = textDrawn.getBBox().height;
	
	textDrawn.remove();
	
	return retObj;
}

Quantae.plotText = function(textX, textY, text, paperToDraw, textProperties)
{
	var retObj = {};

	var textDrawn = paperToDraw.text(textX, textY, text);

	textProperties["text-anchor"] = "start";
	textDrawn.attr(textProperties);

	retObj.textWidth = textDrawn.getBBox().width;
	retObj.textHeight = textDrawn.getBBox().height;
	retObj.textX = textX;
	retObj.textY = textY;
	retObj.textModifiedY = Number((retObj.textHeight/2) + textY);

	textDrawn.attr("y", Number((retObj.textHeight/2) + textY));
	retObj.textObj = textDrawn;
    
    if (Quantae.DebugMode == 1)
    {
    	var rectBox = paperToDraw.rect(textX, textY, retObj.textWidth, retObj.textHeight);
    	rectBox.attr("stroke", "#000000");
    	
    	retObj.outerRect = rectBox;
    }

	return retObj;
}