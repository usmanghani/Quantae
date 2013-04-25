package com.quantae

import com.mongodb.casbah.Imports._
import org.slf4j.LoggerFactory

/**
 * Copyright 2013 Platfora. All rights reserved.
 * User: usman
 * Date: 4/25/13
 * Time: 1:36 AM
 * // TODO: Insert file comments here.
 */
object testingMongo {

  val logger = LoggerFactory.getLogger(testingMongo.getClass)
  def main(args: Array[String]): Unit = {
    val connection = MongoConnection()
    connection.setWriteConcern(WriteConcern.JournalSafe)
    val users = connection("test")("users")
    users.insert(MongoDBObject("a" -> new java.util.Date))
    println("Blah")
    connection.close
  }
}
