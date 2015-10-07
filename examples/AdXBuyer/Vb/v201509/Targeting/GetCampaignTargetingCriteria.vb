' Copyright 2015, Google Inc. All Rights Reserved.
'
' Licensed under the Apache License, Version 2.0 (the "License");
' you may not use this file except in compliance with the License.
' You may obtain a copy of the License at
'
'     http://www.apache.org/licenses/LICENSE-2.0
'
' Unless required by applicable law or agreed to in writing, software
' distributed under the License is distributed on an "AS IS" BASIS,
' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
' See the License for the specific language governing permissions and
' limitations under the License.

Imports Google.Api.Ads.AdWords.Lib
Imports Google.Api.Ads.AdWords.v201509

Imports System
Imports System.Collections.Generic
Imports System.IO

Namespace Google.Api.Ads.AdWords.Examples.VB.v201509
  ''' <summary>
  ''' This code example gets all targeting criteria for a campaign.  To set
  ''' campaign targeting criteria, run AddCampaignTargetingCriteria.vb. To get
  ''' campaigns, run GetCampaigns.vb.
  ''' </summary>
  Public Class GetCampaignTargetingCriteria
    Inherits ExampleBase
    ''' <summary>
    ''' Main method, to run this code example as a standalone application.
    ''' </summary>
    ''' <param name="args">The command line arguments.</param>
    Public Shared Sub Main(ByVal args As String())
      Dim codeExample As New GetCampaignTargetingCriteria
      Console.WriteLine(codeExample.Description)
      Try
        Dim campaignId As Long = Long.Parse("INSERT_CAMPAIGN_ID_HERE")
        codeExample.Run(New AdWordsUser, campaignId)
      Catch e As Exception
        Console.WriteLine("An exception occurred while running this code example. {0}", _
            ExampleUtilities.FormatException(e))
      End Try
    End Sub

    ''' <summary>
    ''' Returns a description about the code example.
    ''' </summary>
    Public Overrides ReadOnly Property Description() As String
      Get
        Return "This code example gets all targeting criteria for a campaign.  To set campaign " & _
            "targeting criteria, run AddCampaignTargetingCriteria.vb. To get campaigns, run " & _
            "GetCampaigns.vb."
      End Get
    End Property

    ''' <summary>
    ''' Runs the code example.
    ''' </summary>
    ''' <param name="user">The AdWords user.</param>
    ''' <param name="campaignId">Id of the campaign from which targeting
    ''' criteria are retrieved.</param>
    Public Sub Run(ByVal user As AdWordsUser, ByVal campaignId As Long)
      ' Get the CampaignCriterionService.
      Dim campaignCriterionService As CampaignCriterionService = user.GetService( _
          AdWordsService.v201509.CampaignCriterionService)

      ' Create the selector.
      Dim selector As New Selector
      selector.fields = New String() {
        CampaignCriterion.Fields.CampaignId, Criterion.Fields.Id,
        Criterion.Fields.CriteriaType, Placement.Fields.PlacementUrl
      }

      selector.predicates = New Predicate() {
        Predicate.Equals(CampaignCriterion.Fields.CampaignId, campaignId.ToString()),
        Predicate.Equals(Criterion.Fields.CriteriaType, "PLACEMENT")
      }

      selector.paging = Paging.Default

      Dim page As New CampaignCriterionPage

      Try
        Do
          ' Get all campaign targets.
          page = campaignCriterionService.get(selector)

          ' Display the results.
          If ((Not page Is Nothing) AndAlso (Not page.entries Is Nothing)) Then
            Dim i As Integer = selector.paging.startIndex
            For Each campaignCriterion As CampaignCriterion In page.entries
              Dim placement As Placement = campaignCriterion.criterion
              Console.WriteLine("{0}) Placement with ID {1} and url {2} was found.", i + 1, _
                 placement.id, placement.url)
              i += 1
            Next
          End If
          selector.paging.IncreaseOffset()
        Loop While (selector.paging.startIndex < page.totalNumEntries)
        Console.WriteLine("Number of placements found: {0}", page.totalNumEntries)
      Catch e As Exception
        Throw New System.ApplicationException("Failed to get placements.", e)
      End Try
    End Sub
  End Class
End Namespace
