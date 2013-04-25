package com.quantae

/**
 * Copyright 2013 Platfora. All rights reserved.
 * User: usman
 * Date: 4/25/13
 * Time: 2:32 AM
 * // TODO: Insert file comments here.
 */
object AnswerDimension extends Enumeration {
  type AnswerDimension = Value
  val Unknown = 0

  // A failure on this becomes Major weakness.
  val Understanding = 1

  // Minor weakness.
  val NounConjugationNumber = 2
  val NounConjugationGender = 3

  // FUTURE: We may or may not use this
  val NounConjugationState = 4

  // Minor weakness.
  val VerbConjugationNumber = 5
  val VerbConjugationGender = 6
  val VerbConjugationTense = 8
  val VerbConjugationPerson = 9

  // FUTURE: We may or may not use this
  val VerbConjugationState = 7

  // This is used to update your contextual/analytical score.
  val GrammaticalAnalysis = 10

  // This is used to update your vocab history.
  val Vocabulary = 11
}

class AnswerChoice {
  val answer: String = ""
//  val grammarRole: GrammarRoleHandle = _
  val dimension = AnswerDimension.Unknown
}