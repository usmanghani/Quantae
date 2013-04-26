resolvers ++= {
  Seq(
    "Mongo Casbah Releases" at "https://oss.sonatype.org/content/repositories/releases/",
    "Mongo Casbah Snapshots" at "https://oss.sonatype.org/content/repositories/snapshots/",
    "JBoss" at "http://repository.jboss.org/maven2/",
    "Maven main" at "http://repo1.maven.org/maven2")
}

libraryDependencies ++= {
  val casbah = "org.mongodb" % "casbah_2.9.2" % "2.6.0"
  Seq(
    "org.slf4j" % "slf4j-api" % "1.6.1",
    "org.slf4j" % "slf4j-simple" % "1.6.1",
    "commons-logging" % "commons-logging" % "1.1.2",
    casbah)
}



