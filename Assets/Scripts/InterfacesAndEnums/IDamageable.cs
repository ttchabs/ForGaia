
/*REFERENCE TO THE IDAMAGEABLE INTERFACE:
 * Title: Using Interfaces in Unity Effectively
 * Author: James Makes Games
 * Date: 26 AUgust 2024
 * Code Version: 1.0
 * Availability:
 
 */
public interface IDamageable
{
    public delegate void DamageReceivedEvent(int damageAmount);
    public event DamageReceivedEvent OnDamageReceived;
    void DamageReceived(int damageAmount);
}