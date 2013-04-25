package com.quantae

/**
 * Copyright 2013 Platfora. All rights reserved.
 * User: usman
 * Date: 4/25/13
 * Time: 2:35 AM
 * // TODO: Insert file comments here.
 */

object RecordingType extends Enumeration {
  type RecordingType = Value
  val Unknown = 0
  val Analytical = 1
  val Contextual = 2
}

object MediaType extends Enumeration {
  type MediaType = Value
  val Video = 0
  val Audio = 1
}

abstract class Recording() {
  def typ;

  val uri: String = ""
  val recordingType = RecordingType.Unknown
}

class AudioRecording extends Recording {
  override def typ = MediaType.Audio
}

class VideoRecording extends Recording {
  override def typ = MediaType.Video
}
