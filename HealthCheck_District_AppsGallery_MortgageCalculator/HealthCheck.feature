Feature: Version check
	as a system administrator
	I want a version health check
	In order to know the Alteryx service is working


Scenario:  Version check
	Given the alteryx service is running at "http://gallery.alteryx.com"
	When I invoke the GET at "api/status"
	Then I see the version binaryVersions/serviceLayer is "8.6.1.42414" and binaryVersions/cloud is "8.6.0.42414"

