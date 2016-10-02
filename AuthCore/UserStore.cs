using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace AuthCore
{
    public class UserStore :
        IUserStore<AuthUser>,
        IUserStore<AuthUser, string>,
        IUserPasswordStore<AuthUser, string>
    {
        private AuthDbContext db;
        public UserStore(AuthDbContext applicationDbContext)
        {
            db = applicationDbContext;
        }
        /// <summary>
        /// ユーザーを作成します。
        /// </summary>
        /// <param name="user">ユーザーオブジェクト</param>
        /// <returns>IdentityResult オブジェクト</returns>
        public Task CreateAsync(AuthUser user)
        {
            // この時点で ID が null だと例外
            // 1 つ以上のエンティティで検証が失敗しました。詳細については 'EntityValidationErrors' プロパティを参照してください。
            // ユーザー名重複チェックが自動的になされ、OK の場合のみここに来る。
            // パスワードは自動的に暗号化済みの状態でここに来る。
            db.AuthUsers.Add(user);
            db.SaveChanges();
            return Task.FromResult(default(object));
        }

        /// <summary>
        /// ユーザーを削除します。
        /// </summary>
        /// <param name="user">ユーザーオブジェクト</param>
        /// <returns>IdentityResult オブジェクト</returns>
        public Task DeleteAsync(AuthUser user)
        {
            AuthUser deletedTargetUser = db.AuthUsers.Find(user.Id);
            db.AuthUsers.Remove(deletedTargetUser);
            db.SaveChanges();
            return Task.FromResult(default(object));
        }

        /// <summary>
        /// ユーザーを Id を指定して取得します。
        /// </summary>
        /// <param name="userId">ユーザー ID</param>
        /// <returns>ユーザーオブジェクト</returns>
        public Task<AuthUser> FindByIdAsync(string userId)
        {
            var result = db.AuthUsers.Find(userId);
            return Task.FromResult(result);
        }

        /// <summary>
        /// ユーザーをユーザー名を指定して取得します。
        /// </summary>
        /// <param name="userName">ユーザー名</param>
        /// <returns>ユーザーオブジェクト</returns>
        public Task<AuthUser> FindByNameAsync(string userName)
        {
            var result = db.AuthUsers.FirstOrDefault(x => x.UserName == userName);
            return Task.FromResult(result);
        }

        /// <summary>
        /// ユーザー情報を更新します。
        /// </summary>
        /// <param name="user">ユーザーオブジェクト</param>
        /// <returns>IdentityResult オブジェクト</returns>
        public Task UpdateAsync(AuthUser user)
        {
            // コントローラーと同じ、db.Entry(user).State = EntityState.Modified; で更新しようとすると、下記のエラー
            // 例外の詳細: System.InvalidOperationException: 同じ型の別のエンティティに同じ主キー値が既に設定されているため、
            // 型 'Identity3.Models.ApplicationUser' のエンティティをアタッチできませんでした。
            // この状況は、グラフ内のエンティティでキー値が競合している場合に 'Attach' メソッドを使用するか、
            // エンティティの状態を 'Unchanged' または 'Modified' に設定すると発生する可能性があります。
            // これは、一部のエンティティが新しく、まだデータベースによって生成されたキー値を受け取っていないことが原因である場合があります。
            // この場合は、'Add' メソッドまたは 'Added' エンティティ状態を使用してグラフを追跡してから、
            // 必要に応じて、既存のエンティティの状態を 'Unchanged' または 'Modified' に設定してください。

            var updatedTargetUser = db.AuthUsers.Find(user.Id);
            updatedTargetUser.UserName = user.UserName;
            // Q : 特別な処理無しで、パスワードはハッシュ化されて更新されるのか？
            // A: されない。よって、UpdateAsync ではパスワードを変更しない。
            //updatedTargetUser.Password = user.Password;
            db.SaveChanges();
            return Task.FromResult(default(object));
        }

        /// <summary>
        /// ユーザーにハッシュ化されたパスワードを設定します。
        /// </summary>
        /// <param name="user">ユーザーオブジェクト</param>
        /// <param name="passwordHash">パスワード文字列（未暗号化?）</param>
        /// <returns>IdentityResult オブジェクト</returns>
        public Task SetPasswordHashAsync(AuthUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(default(object));
        }

        /// <summary>
        /// ユーザーからパスワードのハッシュを取得する
        /// </summary>
        /// <param name="user">ユーザーオブジェクト</param>
        /// <returns>パスワードハッシュ文字列</returns>
        public Task<string> GetPasswordHashAsync(AuthUser user)
        {
            var passwordHash = db.AuthUsers.Find(user.Id).PasswordHash;
            return Task.FromResult(passwordHash);
        }

        /// <summary>
        /// パスワードが設定されている場合に true を返却します。
        /// </summary>
        /// <param name="user">ユーザーオブジェクト</param>
        /// <returns>パスワードが設定されている場合は true、それ以外の場合は false</returns>
        public Task<bool> HasPasswordAsync(AuthUser user)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public void Dispose()
        {
            // プロパティをここで明示的に破棄
            // 参考 → https://aspnet.codeplex.com/SourceControl/latest#Samples/Identity/AspNet.Identity.MySQL/RoleStore.cs
            if (db != null)
            {
                db.Dispose();
                db = null;
            }
        }
    }
}
