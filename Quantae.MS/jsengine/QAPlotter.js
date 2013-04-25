Quantae.QAPlotter = {};

Quantae.QAPlotter.PlottedQuesFrags = [];
Quantae.QAPlotter.PlottedAnswers = [];
Quantae.QAPlotter.BottomMargin_Answers = -15;
Quantae.QAPlotter.LeftMargin_Answers = 5;
Quantae.QAPlotter.QuesFrags = [];
Quantae.QAPlotter.Answers = [];
Quantae.QAPlotter.BlankIndex = 0;
Quantae.QAPlotter.PlottedQuesFrags_Base = 'Quantae_QuesFrag_';
Quantae.QAPlotter.PlottedAnswers_Base = 'Quantae_Answers_';
Quantae.QAPlotter.PlottedAnswers_Parent_Base = 'Quantae_Answers_Base';
Quantae.QAPlotter.PlottedAnswers_SubParent_Base = 'Quantae_Answers_SubParent_Base';
Quantae.QAPlotter.PlottedQuesFrags_Parent_Base = 'Quantae_QuesFrags_Base';

Quantae.QAPlotter.PlottedQuesFrags_Parent = null;
Quantae.QAPlotter.PlottedAnswers_Parent = null;
Quantae.QAPlotter.PlottedAnswers_SubParent = null;
Quantae.QAPlotter.PlottedBlank_Parent = null;
Quantae.QAPlotter.RightAnswer = null;

Quantae.QAPlotter.PlotPage = function(paperToDraw, data, leftToRight)
{
    Quantae.QAPlotter.AnalyzeData(data, leftToRight);
    Quantae.QAPlotter.PlotAnswers();
    Quantae.QAPlotter.PlotQuesFrags(leftToRight);
    
    paperToDraw.appendChild(Quantae.QAPlotter.PlottedAnswers_Parent);
    paperToDraw.appendChild(Quantae.QAPlotter.PlottedQuesFrags_Parent);
}

Quantae.QAPlotter.AnalyzeData = function(data, leftToRight)
{
    var i = (leftToRight) ? 0 : (data.questionFragments.length - 1);
    var getNext = true;
    
    while (getNext)
    {
        Quantae.QAPlotter.QuesFrags.push(data.questionFragments[i]);
        
        if (leftToRight)
        {
            i++;
            getNext = (i < data.questionFragments.length);
        }
        else
        {
            getNext = (i > 0);
            i--;
        }
    }
    
    Quantae.QAPlotter.QuesFrags[data.blankIndex] = "&nbsp;";
    Quantae.QAPlotter.BlankIndex = data.blankIndex;
    
    i = 0;
    
    for ( ; i < data.answerChoices.length; i++)
    {
        Quantae.QAPlotter.Answers.push(data.answerChoices[i]);
    }
    
    Quantae.QAPlotter.RightAnswer = data.correctAnswerChoice;
}

Quantae.QAPlotter.PlotAnswers = function()
{
    var mainAnswersDiv = document.createElement('div');
    mainAnswersDiv.setAttribute('class', 'questionFragments');
    mainAnswersDiv.setAttribute('id', Quantae.QAPlotter.PlottedAnswers_Parent_Base);
    
    var answerContainer = document.createElement('div');
    answerContainer.setAttribute('class', 'answerChoices');
    answerContainer.setAttribute('id', Quantae.QAPlotter.PlottedAnswers_SubParent_Base);
    
    var answerDiv = null;
    
    for (var i = 0; i < Quantae.QAPlotter.Answers.length; i++)
    {
        answerDiv = document.createElement('div');
        answerDiv.setAttribute('class', 'answer');
        answerDiv.setAttribute('id', Quantae.QAPlotter.PlottedAnswers_Base + i);
        answerDiv.innerHTML = Quantae.QAPlotter.Answers[i];
        answerContainer.appendChild(answerDiv);
        answerDiv = null;
    }
    
    Quantae.QAPlotter.PlottedAnswers_SubParent = answerContainer;
    
    mainAnswersDiv.appendChild(answerContainer);
    Quantae.QAPlotter.PlottedAnswers_Parent = mainAnswersDiv;
}

Quantae.QAPlotter.PlotQuesFrags = function(leftToRight)
{
    var i = (leftToRight) ? 0 : (Quantae.QAPlotter.QuesFrags.length - 1);
    var getNext = true;
    
    var mainQuestDiv = document.createElement('div');
    mainQuestDiv.setAttribute('class', 'questionFragment');
    mainQuestDiv.setAttribute('id', Quantae.QAPlotter.PlottedQuesFrags_Parent_Base);
    
    var quest = null;
    
    while (getNext)
    {
        quest = document.createElement('div');
        quest.setAttribute('class', 'questionFragment');
        quest.setAttribute('id', Quantae.QAPlotter.PlottedQuesFrags_Base + i);
        quest.innerHTML = Quantae.QAPlotter.QuesFrags[i];
        mainQuestDiv.appendChild(quest);
        quest = null;
        
        if (leftToRight)
        {
            i++;
            getNext = (i < Quantae.QAPlotter.QuesFrags.length);
        }
        else
        {
            getNext = (i > 0);
            i--;
        }
    }
    
    Quantae.QAPlotter.PlottedQuesFrags_Parent = mainQuestDiv;
}

/*Quantae.QAPlotter.BalanceBlankSpace = function(paperToDraw)
{
    var answersDiv = paperToDraw.getElementById(Quantae.QAPlotter.PlottedAnswers_Parent_Base);
    answersDiv.setAttribute('style', 'bottom: ' + Quantae.QAPlotter.BottomMargin_Answers + 'px');
    
    var subAnswersDiv = paperToDraw.getElementById(Quantae.QAPlotter.PlottedAnswers_SubParent_Base);
    
    var blankDiv = paperToDraw.getElementById(Quantae.QAPlotter.PlottedQuesFrags_Base + Quantae.QAPlotter.BlankIndex);
    blankDiv.setAttribute('style', 'width: ' + subAnswersDiv.width + 'px');
}*/