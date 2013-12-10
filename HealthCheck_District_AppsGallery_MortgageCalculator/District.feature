Feature:  District check
	as a system administrator
	I want a District check
	In order to know the Alteryx service is working


Background:
	 Given the alteryx service is running at "http://gallery.alteryx.com"

Scenario:  Active Districts check
	 When I invoke the GET at "api/districts/"
	 Then I see at least 6 active districts

Scenario: Some District
	 When I invoke GET at "api/Districts" for "Communications District"
	 Then I see that district description contains the text "Need to establish B2B potential by geographic territory?"
