# phpcommit
PHP in Wonderland

## Cloning old repo of PHP
```bash
git clone http://git.php.net/repository/php-src.git
```

## Commits where /ext/zlib/zlib.c was included
```bash
git --no-pager log --pretty=format:"%Cred %H %Cblue %an %aD %Creset %s" -- ext/zlib/zlib.c
```

## Merge to branch master
```bash
 git --no-pager log --pretty=format:"%Cred %H %Cblue %an %aD %Creset %s" --min-parents=2
 ```
 
 ## History of commits
 ```bash
 git --no-pager log --pretty=format:"%Cred %H %Cblue %an %aD %Creset %s"                                    
```

## zlib.c
The folder contains (unsorted for now!) the content of each `zlib.c` version present in the history obtained via
```bash
git --no-pager show [HASH]:ext/zlib/zlib.c
```
