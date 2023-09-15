namespace Characters
{
    public class EnemyOutlineRechanger
    {
        private IInteractable _currentEnemyOutline;

        public void SetEnemy(IInteractable newEnemy)
        {
            _currentEnemyOutline?.SetOutline(false);
            _currentEnemyOutline = newEnemy;
            _currentEnemyOutline?.SetOutline(true);
        }
    }
}