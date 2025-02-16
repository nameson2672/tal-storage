namespace UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Add_ShouldReturnCorrectSum()
        {
            // Arrange
            int a = 5, b = 3;

            // Act
            int result = a + b;

            // Assert
            Assert.Equal(8, result);
        }

        [Fact]
        public void Subtract_ShouldReturnCorrectDifference()
        {
            // Arrange
            int a = 10, b = 4;

            // Act
            int result = a-b;

            // Assert
            Assert.Equal(6, result);
        }
    }
}
