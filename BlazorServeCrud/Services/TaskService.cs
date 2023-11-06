using BlazorServeCrud.Enum;
using BlazorServeCrud.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorServeCrud.Services
{
    public class TaskService: ITaskService
    {
        private readonly DatabaseContext _ctx;
        public TaskService(DatabaseContext databaseContext)
        {
            _ctx = databaseContext;
        }
        public async Task<bool> AddUpdateTaskAsync(DTask task)
        {
            try
            {
                if (task.Id > 0)
                    _ctx.Task.Update(task);
                else
                {
                    task.TaskStatus = DTaskStatus.Active;
                    _ctx.Task.Add(task);
                }
                _ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> AddUpdateUserTaskAsync(DTask task)
        {
            try
            {
                if (task.Id > 0)
                {
                    task.TaskStatus = (DTaskStatus)task.TaskStatusId;
                    _ctx.Task.Update(task);
                    if(task.Comment!=null)
                    {
                        TaskComment taskComment = new TaskComment();
                        taskComment.TaskId = task.Id;
                        taskComment.Comment = task.Comment;
                        taskComment.UserId = task.UserId;
                        _ctx.TaskComment.Add(taskComment);
                    }
                }
                else
                {
                    task.TaskStatus = DTaskStatus.Active;
                    _ctx.Task.Add(task);
                }
                _ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<List<DTask>> GetAllTaskAsync()
        {
            try
            {
               List<DTask> tasks = await _ctx.Task.
                                    Include(m=>m.User).ToListAsync();
               return tasks; 
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<DTask> GetTaskByIdAsync(int id)
        {
            DTask task = await _ctx.Task.FirstOrDefaultAsync(_ => _.Id == id);
            task.TaskComment = await _ctx.TaskComment.Where(_ => _.TaskId == task.Id).ToListAsync();
            task.TaskStatusId = (int)task.TaskStatus;
            return task;
        }
        public async Task<List<DTask>> GetUsersTaskAsync(string username)
        {
            var user = await _ctx.User.FirstOrDefaultAsync(_ => _.Email == username);
            if (user != null)
            {
                return await _ctx.Task.Where(_ => _.UserId == user.Id).ToListAsync();
            }
            else
                return null;
        }
        public async Task<bool> DeleteTaskAsync(int id)
        {
            try
            {
                var task = await _ctx.Task.FindAsync(id);
                if (task == null)
                    return false;
                _ctx.Task.Remove(task);
                _ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
