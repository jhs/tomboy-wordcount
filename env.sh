#!/bin/bash
#
# Set the pkg-config environment.  This script should only be sourced if you installed
# Tomboy in a non-standard location (via the --prefix argument to configure).

# Set this to the *parent* of the prefix where you installed tomboy.  For example,
# if your prefix is /usr/local then this should be "/usr".  (Although I myself prefer
# /usr/local/software/tomboy because I like to use GNU stow.)
tomboy_prefix_parent='/usr/local/software'

if [ $(basename "$0") = 'env.sh' ]; then
    echo "This program should be sourced, not run standalone" >&2
    exit 1
fi

# Maybe it's already here.
if pkg-config --exists tomboy-addins; then
    echo "Environment is okay.  Using: `pkg-config --variable=prefix tomboy-addins`"
else
    # Otherwise, search for it.
    for app in $(find "$tomboy_prefix_parent" -type d -wholename '*/lib/pkgconfig' 2> /dev/null); do
        new_path="$app:$PKG_CONFIG_PATH"
        if env PKG_CONFIG_PATH="$new_path" pkg-config --exists tomboy-addins; then
            echo "Found Tomboy: $app"
            export PKG_CONFIG_PATH="$new_path"
            break
        else
            echo "no: $app"
        fi
    done
fi
