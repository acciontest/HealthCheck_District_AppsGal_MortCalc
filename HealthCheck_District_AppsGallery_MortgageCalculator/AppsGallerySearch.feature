Feature: AppSearchCount
		 Check app search results and count the # of apps returns and check if the app returned matches

Background:
	 Given the alteryx service is running at "http://gallery.alteryx.com"

Scenario Outline: Search applications uses only the first term while ignoring others 
	 When I search for application at "api/apps/gallery/" with search multiple term "<term>"
	 Then I see record-count is <count>
	 Examples: 
		| term               | count |
		| choosing blending  | 1     |
		| blending choosing  | 3     |
	


Scenario: search applications for terms has expected name
	When I search for application at "api/apps/gallery" with search term "choosing a location"
	Then I see primaryapplication.metainfo.name contains "Site Selection Demo"



Scenario: search applications for terms has expected counts
	When I search for application at "api/apps/gallery/" with search term "choosing"
	Then I see record-count is 1



