using System;
using System.IO;
using System.Collections.Generic;
using Xunit;
using SwordConsoleApp;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace SwordConsoleApp.Tests
    {
    public class AnalyseInputFileTests
    {
        [Fact]
        public void AnalyseInputFileCharacterFrequency_ShouldReturnTrueAndOutputCorrectResults()
        {
            // Arrange  
            string testContent = "The quick brown fox jumps over the lazy dog";
            string tempFilePath = Path.GetTempFileName();
            File.WriteAllText(tempFilePath, testContent);

            bool ignoreCase = true;
            var analyser = new AnalyseInputFile(tempFilePath, ignoreCase);

            // Act  
            bool result = analyser.AnalyseInputFileCharacterFrequency();

            // Assert  
            Assert.True(result);

            // Cleanup  
            File.Delete(tempFilePath);
        }

        [Fact]
        public void AnalyseInputFileCharacterFrequency_ShouldHandleNonExistentFileGracefully()
        {
            // Arrange  
            string nonExistentFilePath = "nonexistentfile.txt";
            bool ignoreCase = false;
            var analyser = new AnalyseInputFile(nonExistentFilePath, ignoreCase);

            // Act  
            bool result = analyser.AnalyseInputFileCharacterFrequency();

            // Assert  
            Assert.False(result);
        }

        [Theory]
        [InlineData("The quick brown fox", true, 16, new object[] { new object[] { 'o', 2 }, new object[] { 't', 1 }, new object[] { 'h', 1 }, new object[] { 'e', 1 }, new object[] { 'q', 1 } })]
        [InlineData("The quick brown fox", false, 16, new object[] { new object[] { 'o', 2 }, new object[] { 'T', 1 }, new object[] { 'h', 1 }, new object[] { 'e', 1 }, new object[] { 'q', 1 } })]
        [InlineData(" \t\nThe quick brown fox\r\n", true, 16, new object[] { new object[] { 'o', 2 }, new object[] { 't', 1 }, new object[] { 'h', 1 }, new object[] { 'e', 1 }, new object[] { 'q', 1 } })]
        [InlineData("", true, 0, new object[0])]
        [InlineData("@@@!!!###", true, 9, new object[] { new object[] { '@', 3 }, new object[] { '!', 3 }, new object[] { '#', 3 } })]
        public void AnalyseCharacterFrequency_ShouldReturnCorrectResults(string input, bool ignoreCase, int expectedTotal, object[] expectedTop)
        {
            // Act  
            var result = AnalyseInputFile.AnalyseCharacterFrequency(input, ignoreCase);

            // Assert  
            Assert.Equal(expectedTotal, result.TotalCharacters);
            Assert.Equal(expectedTop.Length, result.TopCharacters.Count);

            for (int i = 0; i < expectedTop.Length; i++)
            {
                var expectedPair = (object[])expectedTop[i];
                Assert.Equal((char)expectedPair[0], result.TopCharacters[i].Character);
                Assert.Equal((int)expectedPair[1], result.TopCharacters[i].Count);
            }
        }


        [Fact]
            public void AnalyseCharacterFrequency_ShouldHandleLargeInput()
            {
                // Arrange  
                string largeInput = new string('a', 1000) + new string('b', 500) + new string('c', 200);

                // Act  
                var result = AnalyseInputFile.AnalyseCharacterFrequency(largeInput, true);

                // Assert  
                Assert.Equal(1700, result.TotalCharacters);
                Assert.Equal(3, result.TopCharacters.Count);
                Assert.Equal(('a', 1000), result.TopCharacters[0]);
                Assert.Equal(('b', 500), result.TopCharacters[1]);
                Assert.Equal(('c', 200), result.TopCharacters[2]);
            }
        }
    }
